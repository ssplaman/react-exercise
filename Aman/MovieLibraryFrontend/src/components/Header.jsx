import { useState, useEffect } from 'react'

import GenreDropdown from './GenreDropdown';
import SortMedia from './SortMedia';

const Header = ({ searchTerm, onSearch,
    mediaType, onMediaTypeChange,
    selectedGenreId, selectedSortId }) => {

    const [input, setInput] = useState(searchTerm);

    useEffect(() => {
        setInput(searchTerm);
    }, [searchTerm]);

    const handleSearchSubmit = (e) => {
        e.preventDefault();
        onSearch(input);
    };

    const isSearchDisabled = !!selectedGenreId || !!selectedSortId;

    return (
        <>
            <div className="media-type">
                <form className="search-bar" onSubmit={handleSearchSubmit}>
                    <input
                        type="text"
                        placeholder="Search..."
                        onChange={(e) => setInput(e.target.value)}
                        disabled={isSearchDisabled}
                        value={input} />
                    <button type="submit" disabled={isSearchDisabled}>Search</button>
                </form>

                <div className="media-type-things">
                    <div className="media-type-buttons">
                        <button
                            className={`movie-button ${mediaType === "movie" ? "active" : ""}`}
                            onClick={() => onMediaTypeChange("movie")}>
                            Movies
                        </button>

                        <button
                            className={`tv-series-button ${mediaType === "tv" ? "active" : ""}`}
                            onClick={() => onMediaTypeChange("tv")}>
                            TV Series
                        </button>
                    </div>
                    <div className="genre-sort-dropdowns">
                        <GenreDropdown mediaType={mediaType} disabled={!!selectedSortId} />

                        <SortMedia disabled={!!selectedGenreId} />
                    </div>
                </div>
            </div>
        </>
    )
}

export default Header;
