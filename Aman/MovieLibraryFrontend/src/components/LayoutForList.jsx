import { Link } from "react-router-dom"

const LayoutForList = ({ children }) => {
    return (
        <div className="wrapper">
            <header>
                <h1><Link to="/" style={{
                        textDecoration: 'none',
                        cursor: 'pointer',
                        color: '#fff'
                    }}>🎬 Library</Link></h1>
            </header>
            {children}
        </div>
    )
}

export default LayoutForList