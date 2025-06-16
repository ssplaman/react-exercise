import React from 'react'
import { CanvasJSChart } from 'canvasjs-react-charts';

type DataPoint = {
    x: Date;
    y: number;
}

interface ExchangeGraphCanvasProps {
    dataPoints: DataPoint[];
    base: string;
    target: string;
}

const ExchangeGraphCanvas: React.FC<ExchangeGraphCanvasProps> = ({ dataPoints, base, target }) => {
    const options = {
        animationEnabled: true,
        title: {
            text: `Exchange Rate: ${base} to ${target} of 30 days `,
        },
        axisX: {
            valueFormatString: "DD MMM YYYY",
        },
        axisY: {
            title: `Exchange Rate`,
            includeZero: false,
        },
        data: [
            {
                type: "splineArea" as const,
                color: "rgba(0, 0, 0, .54)",
                markerSize: 5,
                xValueFormatString: "DD MMM YYYY",
                dataPoints,
            },
        ]
    }

    return (
        <div className="mt-4">
            <CanvasJSChart options={options} />
        </div>
    )
}

export default ExchangeGraphCanvas