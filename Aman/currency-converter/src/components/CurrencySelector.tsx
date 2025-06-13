import React from "react";
import Dropdown from "./Dropdown"

type CountryCurrency = {
    code: string;
    name: string;
}

interface CurrencySelectorProps {
    id: string;
    onChange: (value: string) => void;
    value: string;
    currencies: CountryCurrency[];
}

const CurrencySelector: React.FC<CurrencySelectorProps> = ({ id, onChange, value, currencies }) => {
    return (
        <div className="col">
            <label className="form-label">{id}</label>
            <Dropdown
                id={id}
                onChange={onChange}
                value={value}
                currencies={currencies} />
        </div>
    )
}

export default CurrencySelector