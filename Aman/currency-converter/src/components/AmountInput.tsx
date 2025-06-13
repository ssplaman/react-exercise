import React from "react";

interface AmountInputProps {
    amount: number;
    setAmount: (amount: number) => void;
}

const AmountInput: React.FC<AmountInputProps> = ({ amount, setAmount }) => {
    return (
        <div className="mb-3">
            <label className="form-label">Amount</label>
            <input
                type="number"
                id="amount"
                className="form-control"
                value={amount}
                onChange={e => setAmount(Number(e.target.value))} />
        </div>
    )
}

export default AmountInput