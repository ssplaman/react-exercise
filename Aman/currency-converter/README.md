# Currency Converter

A modern currency converter built with **React**, **TypeScript**, **Redux**, and **Vite**. This app allows you to convert an amount from one currency to another, view the result instantly, and keep a history of all your conversions.

## Features

- **Amount Input:** Enter the amount you want to convert.
- **Currency Selection:** Two dropdowns for selecting the "From" and "To" currencies (country with currency code).
  - Default values:  
    - Amount: `1`
    - From: `INR` (Indian Rupee)
    - To: `USD` (US Dollar)
- **Convert Button:** Click to perform the conversion using live rates.
- **Result Display:** The conversion result is shown below the button.
- **Conversion History:** Every conversion is saved and displayed in a history list, with the most recent conversion at the top.
- **Historical Chart:** A line chart shows the exchange rate trend for the past 30 days based on the selected currency pair.

## Technologies Used

- [React](https://react.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Redux Toolkit](https://redux-toolkit.js.org/)
- [Vite](https://vitejs.dev/) (for fast development)
- [Axios](https://axios-http.com/) (for API requests)
- [CanvasJS](https://canvasjs.com/) (used for rendering the historical graph)

## Getting Started

### Prerequisites

- [Node.js](https://nodejs.org/) (v16 or higher recommended)
- [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)

### Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/ssplaman/react-exercise.git
   change branch to feature/aman
   cd currency-converter
   ```

2. **Install dependencies:**
   ```sh
   npm install
   # or
   yarn install
   ```

3. **Start the development server:**
   ```sh
   npm run dev
   # or
   yarn dev
   ```

4. Open [http://localhost:5173](http://localhost:5173) in your browser.

## Project Structure

```
src/
  App.tsx                # Main application component
  main.tsx               # Entry point
  components/            # UI components (AmountInput, CurrencySelector, ConvertButton, etc.)
  redux/
    store.ts             # Redux store setup
    slices/
      currencySlice.ts   # Currency conversion state and reducers
  utlis/
    config.ts             # Configuration for API calling
public/
  vite.svg               # Static assets
```

## How It Works

1. **Enter Amount:**  
   Input the amount you want to convert.

2. **Select Currencies:**  
   Choose the source ("From") and target ("To") currencies from the dropdowns.

3. **Convert:**  
   Click the "Convert" button. The app fetches the latest exchange rate and displays the result below the button.

4. **Conversion History:**  
   Each conversion is added to the history list, so you can see all your previous conversions.

4. **View Historical Graph:**  
   A 30-day line chart is displayed below the result, showing how the exchange rate changed over time between the selected currencies.

## Default Values

- **Amount:** `1`
- **From Currency:** `INR`
- **To Currency:** `USD`

## API

### Live Conversion

- Uses [Open Exchange Rates API](https://openexchangerates.org/) for real-time currency rates.
- **API Key Required:**
You must create a `.env` file in the project root with your API credentials:
```env
VITE_OXR_BASE_URL=https://openexchangerates.org/api
VITE_OXR_API_KEY=your_openexchangerates_api_key
```
Replace `your_openexchangerates_api_key` with your actual API key from Open Exchange Rates.

### Historical Data (Last 30 Days)

- Uses [RapidAPI Currency Conversion and Exchange Rates API](https://rapidapi.com/principalapis/api/currency-conversion-and-exchange-rates/playground/endpoint_01c2f371-2ab0-4e56-98f4-e4f4149e9cfc) to fetch exchange rate history.
- **API Key Required:**
You must create a `.env` file in the project root with your API credentials:
```env
VITE_HISTORY_CURRENCY_BASE_URL=https://currency-conversion-and-exchange-rates.p.rapidapi.com
VITE_HISTORY_CURRENCY_API_KEY=your_rapid_api_key
```
Replace `your_rapid_api_key` with your actual API key from RapidAPI.

## License

This project is licensed under the MIT License.

---

Made with ❤️ using React + TypeScript + Redux.