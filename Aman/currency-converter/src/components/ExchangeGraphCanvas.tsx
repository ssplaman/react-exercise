import React from 'react'
import {
    Chart as ChartJS,
    LineElement,
    PointElement,
    LinearScale,
    TimeScale,
    Title,
    Tooltip,
    Legend,
    Filler,
} from 'chart.js';
import { Line } from 'react-chartjs-2';
import 'chartjs-adapter-date-fns';

type DataPoint = {
    x: Date;
    y: number;
}

interface ExchangeGraphCanvasProps {
    dataPoints: DataPoint[];
    base: string;
    target: string;
}

ChartJS.register(
    LineElement,
    PointElement,
    LinearScale,
    TimeScale,
    Title,
    Tooltip,
    Legend,
    Filler
);

const ExchangeGraphCanvas: React.FC<ExchangeGraphCanvasProps> = ({ dataPoints, base, target }) => {
    const chartData = {
        datasets: [
            {
                label: `${base} to ${target}`,
                data: dataPoints,
                fill: true,
                borderColor: '#ff6384',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderWidth: 2,
                pointRadius: 4,
                pointHoverRadius: 6,
                tension: 0.4
            }
        ]
    };

    const options = {
        responsive: true,
        plugins: {
            title: {
                display: true,
                text: `Exchange Rate: ${base} to ${target} of 30 days`
            },
            tooltip: {
                mode: 'index' as const,
                intersect: false
            },
            legend: {
                display: false
            }
        },
        scales: {
            x: {
                type: 'time' as const,
                time: {
                    unit: 'day' as const,
                    tooltipFormat: 'dd MMM yyyy',
                    displayFormats: {
                        day: 'dd MMM'
                    }
                },
                title: {
                    display: true,
                    text: 'Date'
                },
                ticks: {
                    autoSkip: true,
                    maxRotation: 0,
                    minRotation: 0,
                    maxTicksLimit: 10
                },
                grid: {
                    display: false
                }
            },
            y: {
                title: {
                    display: true,
                    text: 'Exchange Rate'
                },
                ticks: {
                    callback: (value: unknown) => {
                        if (typeof value === 'number')
                            return value.toFixed(5)
                        return String(value);
                    }
                },
                grid: {
                    color: '#e0e0e0'
                },
                beginAtZero: false
            }
        }
    };

    return (
        <div className="mt-4">
            <Line data={chartData} options={options} />
        </div>
    )
}

export default ExchangeGraphCanvas