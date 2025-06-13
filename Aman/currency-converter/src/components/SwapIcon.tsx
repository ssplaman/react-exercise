import React from "react";

interface SwapIconProps {
    onSwap: () => void;
}

const SwapIcon: React.FC<SwapIconProps> = ({ onSwap }) => {
    return (
        <div className="col-auto text-center">
            <span
                style={{ fontSize: '1.5rem', cursor: 'pointer' }}
                title="Swap"
                onClick={onSwap}>
                ⇄
            </span>
        </div>
    )
}

export default SwapIcon