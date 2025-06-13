import { useEffect, useState } from 'react'
import axios from 'axios';
import { useDispatch, useSelector } from 'react-redux';

import AmountInput from './components/AmountInput'
import CurrencySelector from './components/CurrencySelector'
import SwapIcon from './components/SwapIcon'
import ConvertButton from './components/ConvertButton'
import ResultDisplay from './components/ResultDisplay'
import HistoryList from './components/HistoryList'

import type { RootState } from './redux/store';
import { setAmount, setFromCurrency, setToCurrency, addToHistory } from './redux/slices/currencySlice';

type CountryCurrency = {
  code: string;
  name: string;
}

const config = {
  headers: {
    Authorization: `Token ${import.meta.env.VITE_CURRENCY_API_KEY}`,
  }
};

const CurrencyConverter = () => {
    const dispatch = useDispatch();
    const { fromCurrency, toCurrency, amount, history } = useSelector((state: RootState) => state.counter);

    const [currencies, setCurrencies] = useState<CountryCurrency[]>([]);
    const [result, setResult] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const convertCurrency = async () => {
        setIsLoading(true);
        if (!fromCurrency || !toCurrency || amount <= 0) {
            setResult('Please enter valid amount and select both currencies.');
            setIsLoading(false);
            return;
        } else if (fromCurrency === toCurrency) {
            setResult(`No conversion needed: ${amount} ${fromCurrency} is equal to ${amount} ${toCurrency}.`);
            setIsLoading(false);
            return;
        }

        try {
            const response = await axios.get(`${import.meta.env.VITE_CURRENCY_BASE_URL}/latest.json?symbols=${fromCurrency}%2C${toCurrency}`, config);

            if (response.status === 200) {
                const rates = response.data.rates;
                if (!rates[fromCurrency] || !rates[toCurrency]) {
                    setResult('Currency not supported.');
                    setIsLoading(false);
                    return;
                }

                const someAmount = amount / rates[fromCurrency];
                const converted = (someAmount * rates[toCurrency]).toFixed(2);
                const conversionResult = `${amount} ${fromCurrency} = ${converted} ${toCurrency}`;

                setResult(conversionResult);
                dispatch(addToHistory(conversionResult));
            } else {
                setResult('Failed');
                setIsLoading(false);
                return;
            }
        } catch (error) {
            console.error('Conversion error:', error);
            setResult('Conversion failed. Please try again.');
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        async function fetchCountryCurrencies() {
            try {
                const response = await axios.get(`${import.meta.env.VITE_CURRENCY_BASE_URL}/currencies.json`, config);

                if (response.status === 200) {
                    const data = response.data;

                    if (!data || typeof data != 'object') {
                        setCurrencies([]);
                        return;
                    }

                    const currenciesArray = Object.entries(data)
                        .filter(([code, name]) => code && name)
                        .map(([code, name]) => ({
                            code,
                            name: String(name),
                        }));

                    currenciesArray.sort((a, b) => a.code.localeCompare(b.code));

                    setCurrencies(currenciesArray);
                } else {
                    setCurrencies([]);
                    return;
                }
            } catch (error) {
                console.error('Error fetching country currencies:', error);
                setCurrencies([]);
            }
        };

        fetchCountryCurrencies();
    }, []);

    const handleSwap = () => {
        dispatch(setFromCurrency(toCurrency));
        dispatch(setToCurrency(fromCurrency));
    };

    return (
        <div className="converter">
            <h1 className="text-center mb-4">Currency Converter</h1>
            <AmountInput amount={amount} setAmount={(val) => dispatch(setAmount(val))} />

            <div className="row mb-3 align-items-center">
                <CurrencySelector
                    id="From"
                    onChange={(val) => dispatch(setFromCurrency(val))}
                    currencies={currencies}
                    value={fromCurrency} />
                <SwapIcon onSwap={handleSwap} />
                <CurrencySelector
                    id="To"
                    onChange={(val) => dispatch(setToCurrency(val))}
                    currencies={currencies}
                    value={toCurrency} />
            </div>

            <ConvertButton onClick={convertCurrency} isLoading={isLoading} />
            <ResultDisplay result={result} />

            {history.length > 0 && (
                <HistoryList history={history} />
            )}
        </div>
    )
}

export default CurrencyConverter