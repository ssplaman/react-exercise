import { useEffect, useRef, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { createPortal } from "react-dom";

import styles from "../css/movieDetail.module.css";
import ShareModal from "../components/ShareModal.jsx";
import Loader from "../components/Loader.jsx";
import NotFound from "../components/NotFound.jsx";
import ReviewSection from "../components/ReviewSection.jsx";

import { fetchMediaDetail, fetchMediaTrailer } from "../api.jsx";
import { envVar } from "../utils/env-var";

const MovieDetail = () => {
    const { mediaType, id } = useParams();

    const [mediaDetail, setMediaDetail] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [isSharePopupOpen, setIsSharePopupOpen] = useState(false);
    const [isMediaFound, setIsMediaFound] = useState(true);
    const [trailerUrl, setTrailerUrl] = useState("");
    const shareButtonRef = useRef(null);

    useEffect(() => {
        fetchDetail(mediaType, id);
        setIsSharePopupOpen(false);
        fetchTrailer(mediaType, id);
    }, [id, mediaType]);

    const fetchDetail = async (mediaType, movieId) => {
        setIsLoading(true);

        const response = await fetchMediaDetail(mediaType, movieId);
        // if (response.status === 200 && response.data) {
        if (response.status === 200 && response.data.data) {
            // setMediaDetail(response.data);
            setMediaDetail(response.data.data);
            setIsMediaFound(true);
        } else {
            setIsMediaFound(false);
            setMediaDetail(null);
        }

        setIsLoading(false);
    };

    const fetchTrailer = async (mediaType, movieId) => {
        const response = await fetchMediaTrailer(mediaType, movieId);
        // if (response.status === 200 && response.data) {
        if (response.status === 200 && response.data.data) {
            // const videos = response.data.results || [];
            // const officialTrailer = videos.find(
            //     (video) => video.name === "Official Trailer" && video.site === "YouTube"
            // );
            
            // if (officialTrailer) {
            //     setTrailerUrl(`https://www.youtube.com/watch?v=${officialTrailer.key}`);
            // }
            const videos = response.data.data || [];
            if(videos.key){
                setTrailerUrl(`https://www.youtube.com/watch?v=${videos.key}`);
            }
        } else {
            console.error("Failed to fetch trailer");
        }
    }

    const handleShare = (e) => {
        e.preventDefault();
        setIsSharePopupOpen(true);
    };

    if (isLoading) return <Loader />;
    if (!isMediaFound) return <NotFound />;

    return (
        <>
            <div className={styles["movie-page-container"]}>
                <div className={styles["movie-banner"]}>
                    <img
                        src={`${envVar.tmdbApi.image.baseUrl}/w1280${mediaDetail?.backdrop_path}`}
                        alt="Background Poster"
                        className={styles["movie-banner-img"]}
                    />
                    <div className={styles.overlay}></div>
                </div>

                <div className={styles.description}>
                    <div className={styles["movie-content"]}>
                        <img src={`${envVar.tmdbApi.image.baseUrl}/w300${mediaDetail?.poster_path}`} alt="Movie Poster" className={styles["movie-poster-detail-page"]} />
                        <div className={styles["movie-info-detail-page"]}>
                            <h2 className={styles["movie-title-detail-page"]}>
                                {mediaDetail?.title ?? mediaDetail?.name}
                            </h2>
                            <p className={styles["movie-release"]}>
                                Release: {mediaDetail?.release_date ?? mediaDetail?.first_air_date}
                            </p>
                            <p className={styles["movie-rating-detail-page"]}>
                                ⭐ {mediaDetail?.vote_average}
                            </p>

                            {/* Genres */}
                            <div className={styles["movie-genres"]}>
                                <strong>Genres</strong>
                                <ul className={styles["genres-list"]}>
                                    {mediaDetail?.genres.map((item) => (
                                        <li key={item.id}>
                                            <span className={styles["genre-badge"]}>{item.name}</span>
                                        </li>
                                    ))}
                                </ul>
                            </div>

                            <p className={styles["movie-description-detail-page"]}>
                                {mediaDetail?.overview}
                            </p>

                            <div className={styles["movie-actions"]}>
                                <button onClick={(e) => handleShare(e)} className={styles["share-button"]} ref={shareButtonRef}>
                                    Share
                                </button>
                                {isSharePopupOpen &&
                                    createPortal(
                                        <ShareModal onClose={() => {
                                            setIsSharePopupOpen(false);
                                            shareButtonRef.current?.focus();
                                        }} />,
                                        document.getElementById("modal-root")
                                    )}
                                <Link to={`/${mediaType}/${id}/gallery`} className={`${styles["movie-link"]} ${styles["share-button"]}`}>
                                    Gallery
                                </Link>
                                <Link to={`/${mediaType}/${id}/similar`} className={`${styles["movie-link"]} ${styles["share-button"]}`}>
                                    {`${mediaType === "movie" ? "Similar Movies" : "Similar Shows"}`}
                                </Link>
                                {trailerUrl &&
                                    <Link to={trailerUrl} className={`${styles["movie-link"]} ${styles["share-button"]}`} target="_blank" rel="noopener noreferrer">
                                        Watch Trailer
                                    </Link>}
                            </div>
                        </div>
                    </div>

                    <div style={{ padding: 32 }}>
                        <ReviewSection mediaType={mediaType} id={id} styles={styles} />
                    </div>

                </div>
            </div>
        </>
    );
};

export default MovieDetail;
