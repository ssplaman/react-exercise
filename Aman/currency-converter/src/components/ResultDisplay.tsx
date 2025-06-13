import React from "react";

interface ResultDisplayProps {
    result: string;
}

const ResultDisplay: React.FC<ResultDisplayProps> = ({ result }) => {
    return (
        <h3 className="mt-4 text-center" id="result">
            {result}
        </h3>
    )
}

export default ResultDisplay