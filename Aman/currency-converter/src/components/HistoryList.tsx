import React from "react";

interface HistoryListProps {
    history: string[];
}

const HistoryList: React.FC<HistoryListProps> = ({ history }) => {
    return (
        <div className="mt-4">
            <h4>Conversion History</h4>
            <div>
                {history.map((item, index) => (
                    <div key={index}>{item}</div>
                ))}
            </div>
        </div>
    )
}

export default HistoryList