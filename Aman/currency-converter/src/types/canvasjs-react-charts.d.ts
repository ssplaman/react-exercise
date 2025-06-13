declare module 'canvasjs-react-charts' {
    import * as React from 'react';

    interface CanvasJSChartProps {
        options: unknown;
        onRef?: (ref: React.RefObject<unknown>) => void;
    }

    export class CanvasJSChart extends React.Component<CanvasJSChartProps> { }

    const CanvasJSReact: {
        CanvasJSChart: typeof CanvasJSChart;
    };

    export default CanvasJSReact;
}
