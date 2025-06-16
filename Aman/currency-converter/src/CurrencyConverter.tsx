import { useEffect, useState } from 'react'
import axios from 'axios'
import { useDispatch, useSelector } from 'react-redux';

import ExchangeGraphCanvas from './components/ExchangeGraphCanvas'
import HistoryList from './components/HistoryList'
import ResultDisplay from './components/ResultDisplay'
import ConvertButton from './components/ConvertButton'
import CurrencySelector from './components/CurrencySelector'
import SwapIcon from './components/SwapIcon'
import AmountInput from './components/AmountInput'

import type { RootState } from './redux/store';
import { setAmount, setFromCurrency, setToCurrency, addToHistory } from './redux/slices/currencySlice';
import { config, rapidApiConfig, type CountryCurrency, type DataPoint } from './utils/config'

const CurrencyConverter = () => {
    const dispatch = useDispatch();
    const { fromCurrency, toCurrency, amount, history } = useSelector((state: RootState) => state.counter);

    const [currencies, setCurrencies] = useState<CountryCurrency[]>([]);
    const [result, setResult] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [dataPoints, setDataPoints] = useState<DataPoint[]>([]);
    const [isGraphLoading, setIsGraphLoading] = useState(true);
    const [error, setError] = useState('');

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
                const converted = (someAmount * rates[toCurrency]).toFixed(4);
                const conversionResult = `${amount} ${fromCurrency} = ${converted} ${toCurrency}`;

                setResult(conversionResult);
                dispatch(addToHistory(conversionResult));

                await fetchHistoricalData();
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

    const fetchHistoricalData = async () => {
        setIsGraphLoading(true);
        setError('');

        const days = 30;
        const end = new Date();
        const start = new Date();
        start.setDate(end.getDate() - days);

        const formatDate = (d: Date) => d.toISOString().split("T")[0];
        const startDate = formatDate(start);
        const endDate = formatDate(end);

        try {
            const response = await axios.get(`${import.meta.env.VITE_HISTORY_CURRENCY_BASE_URL}/timeseries?start_date=${startDate}&end_date=${endDate}&base=${fromCurrency}&symbols=${toCurrency}`,
                rapidApiConfig
            );

            const rates = response.data.rates;

            const points: DataPoint[] = Object.entries(rates)
                .map(([date, rateObj]) => {
                    const rate = (rateObj as Record<string, number>)[toCurrency];
                    if (!rate) return null;
                    return {
                        x: new Date(date),
                        y: parseFloat(rate.toFixed(4)),
                    };
                })
                .filter((point): point is DataPoint => point !== null);

            points.sort((a, b) => a.x.getTime() - b.x.getTime());

            setDataPoints(points);
        } catch (error) {
            console.error("Error fetching historical data:", error);
            if (error instanceof Error) {
                setError(error.message);
            } else {
                setError("An unknown error occurred.");
            }
            setDataPoints([]);
        } finally {
            setIsGraphLoading(false);
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

    const changeAmount = (val: number) => {
        if (val < 0) {
            return dispatch(setAmount(0));
        }
        return dispatch(setAmount(val));
    };

    return (
        <div className="converter">
            <h1 className="text-center mb-4">Currency Converter</h1>
            <AmountInput amount={amount} setAmount={(val) => changeAmount(val)} />

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

            {result &&
                <ResultDisplay result={result} />}

            {history.length > 0 && (
                <HistoryList history={history} />
            )}

            {(!isGraphLoading && dataPoints.length > 0) ?
                <ExchangeGraphCanvas
                    dataPoints={dataPoints}
                    base={fromCurrency}
                    target={toCurrency} />
                : error &&
                <div className="mt-4">
                    <h4>Error fetching historical data: {error}</h4>
                </div>}
        </div>
    )
}

export default CurrencyConverter