import { Link } from "react-router-dom"

const LayoutForDetail = ({ children }) => {
    return (
        <>
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
            <div id="modal-root"></div>
        </>

    )
}

export default LayoutForDetail