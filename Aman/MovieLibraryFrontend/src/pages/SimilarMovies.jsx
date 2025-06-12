import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate, useParams } from "react-router-dom";

import styles from '../css/movieGallery.module.css';
import Loader from "../components/Loader";
import FailedToFetchMovies from "../components/FailedToFetchMovies";
import Pagination from "../components/Pagination";

import { fetchSimilarMediaList } from "../api";
import { envVar } from "../utils/env-var";

const SimilarMovies = () => {
    const { mediaType, id } = useParams();
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const page = queryParams.get("page") || 1;
    const navigate = useNavigate();

    const [isLoading, setIsLoading] = useState(true);
    const [similarMovies, setSimilarMovies] = useState([]);
    const [totalPage, setTotalPage] = useState(0);
    const [currentPage, setCurrentPage] = useState(Number(page));
    const [fetchError, setFetchError] = useState("");

    const fetchSimilarMovies = async (mediaType, movieId, pageNumber) => {
        setIsLoading(true);
        setFetchError("");

        const response = await fetchSimilarMediaList(mediaType, movieId, pageNumber);
        
        if (response.status === 200) {
            // setSimilarMovies(response.data.results);
            // setTotalPage(response.data.total_pages);
            setSimilarMovies(response.data.data.results);
            setTotalPage(response.data.data.total_pages);
        } else if (response.status === 400) {
            // const message = response?.response?.data?.status_message || "Unexpected error occurred.";
            const message = response?.response?.data?.errorDetails || "Unexpected error occurred.";
            setSimilarMovies([]);
            setFetchError(message)
        } else {
            setSimilarMovies([]);
        }

        setIsLoading(false);
    };

    useEffect(() => {
        fetchSimilarMovies(mediaType, id, currentPage);
    }, [id, currentPage, mediaType]);

    const handlePageChange = (page) => {
        setCurrentPage(page);
        navigate(`?page=${page}`);
    };

    return isLoading ? (
        <Loader />
    ) : fetchError ? (
        <>
            <FailedToFetchMovies message={fetchError} />
            <Pagination
                currentPage={currentPage}
                totalPage={totalPage}
                onPageChange={handlePageChange}
            />
        </>
    ) : (
        <>
            <div className={styles["gallery-wrapper"]}>
                {similarMovies && similarMovies.length > 0 ? (
                    <div className={styles.gallery} id="gallery">
                        {similarMovies.map((item) => (
                            <Link to={`/movie/${item.id}?page=${currentPage || 1}`} className={styles["movie-link"]} key={item.id}>
                                <div className={styles["movie-card"]}>
                                    <img src={`${envVar.tmdbApi.image.baseUrl}/w200${item.poster_path}`}
                                        alt="Movie Poster" style={{ borderRadius: '0', height: '448px' }} />
                                    <div className={styles["movie-content"]}>
                                        <div className={styles["movie-title"]}>{item.title ?? item.name}</div>
                                    </div>
                                </div>
                            </Link>
                        ))}
                    </div>
                ) : (
                    <FailedToFetchMovies message={"No similar movies available."} />
                )}
            </div >
            <Pagination
                currentPage={currentPage}
                totalPage={totalPage}
                onPageChange={handlePageChange}
            />
        </>
    )
}

export default SimilarMovies