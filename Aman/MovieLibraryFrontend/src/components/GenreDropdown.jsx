import { useState, useEffect } from 'react';
import { useSearchParams } from "react-router-dom";
import { fetchGenreListOfMedia } from '../api';

const GenreDropdown = ({ mediaType, disabled }) => {
    const [genres, setGenres] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [fetchError, setFetchError] = useState('');
    const [selectedGenre, setSelectedGenre] = useState(null);
    const [searchParams, setSearchParams] = useSearchParams();

    useEffect(() => {
        const fetchGenre = async () => {

            setIsLoading(true);

            const response = await fetchGenreListOfMedia(mediaType);

            if (response?.status === 200) {
                // setGenres(response.data.genres);
                setGenres(response.data.data.genres);
                setFetchError('');
            } else {
                // const message = response?.response?.data?.status_message || 'Unexpected error occurred.';
                const message = response?.response?.data?.errorDetails || 'Unexpected error occurred.';
                setGenres([]);
                setFetchError(message);
            }

            setIsLoading(false);
        };

        fetchGenre();
    }, [mediaType]);

    useEffect(() => {
        const initialGenre = searchParams.get("genreId");
        setSelectedGenre(initialGenre ? Number(initialGenre) : null);
    }, [searchParams]);

    const handleChange = (e) => {
        const genreId = e.target.value ? Number(e.target.value) : null;
        setSelectedGenre(genreId);

        const params = Object.fromEntries([...searchParams]);
        if (genreId === null) {
            delete params.genreId;
        } else {
            params.genreId = genreId;
            delete params.search;
        }
        params.page = 1;
        setSearchParams(params);
    };

    return (
        <div className="genre-select-wrapper">
            {fetchError ? (
                <div className="genre-error">{fetchError}</div>
            ) : (
                <select className="genre-select"
                    value={selectedGenre ?? ""}
                    onChange={handleChange}
                    disabled={disabled}
                >
                    <option value="">
                        Reset
                    </option>
                    {isLoading ? (
                        <option disabled>
                            Loading...
                        </option>
                    ) : (
                        genres.map((genre) => (
                            <option key={genre.id} value={genre.id}>
                                {genre.name}
                            </option>
                        ))
                    )}
                </select>
            )}
        </div>
    )
}

export default GenreDropdown