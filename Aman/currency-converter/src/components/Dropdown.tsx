import React from "react";

type CountryCurrency = {
    code: string;
    name: string;
}

interface DropdownProps {
    id: string;
    onChange: (value: string) => void;
    value: string;
    currencies: CountryCurrency[];
}

const Dropdown: React.FC<DropdownProps> = ({ id, onChange, value, currencies }) => {
    return (
        <select id={id} className="form-select" onChange={(e) => onChange(e.target.value)} value={value}>
            <option value="">Select Currency</option>
            {currencies.map(({ code, name }) => (
                <option key={code} value={code}>
                    {code} - {name}
                </option>
            ))}
        </select>
    )
}

export default Dropdown