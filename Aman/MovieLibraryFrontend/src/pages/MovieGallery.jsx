import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { createPortal } from "react-dom";

import styles from '../css/movieGallery.module.css';
import MovieGalleryImage from '../components/MovieGalleryImage';
import Loader from "../components/Loader";
import FailedToFetchMovies from "../components/FailedToFetchMovies";
import ImageModal from "../components/ImageModal";

import { fetchMediaImages } from "../api";

const MovieGallery = () => {
    const navigate = useNavigate();

    const { mediaType, id } = useParams();
    const [isLoading, setIsLoading] = useState(true);
    const [galleryImages, setGalleryImages] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [currentImageIndex, setCurrentImageIndex] = useState(0);
    const [selectedImageIndex, setSelectedImageIndex] = useState(null);
    const [showAllImages, setShowAllImages] = useState(false);

    const fetchGalleryImages = async (mediaType, movieId) => {
        setIsLoading(true);

        const response = await fetchMediaImages(mediaType, movieId)
        if (response.status === 200) {
            setGalleryImages(response?.data?.data?.posters);
        } else {
            setGalleryImages([]);
        }

        setIsLoading(false);
    };

    useEffect(() => {
        fetchGalleryImages(mediaType, id);
    }, [id, mediaType]);

    const handleImageClick = (index) => {
        setCurrentImageIndex(index);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setSelectedImageIndex(currentImageIndex);
    };

    const handleImageSlider = (direction) => {
        setCurrentImageIndex(direction);
    };

    const visibleImages = showAllImages || galleryImages.length <= 20
        ? galleryImages
        : galleryImages.slice(0, 20);

    return isLoading ? (
        <Loader />
    ) : (
        <div className={styles["gallery-wrapper"]}>
            <button
                className={styles["back-button"]}
                onClick={() => navigate(-1)}
            >
                ⬅ Back
            </button>

            {galleryImages && galleryImages.length > 0 ? (
                <>
                    <div className={styles.gallery} id="gallery">
                        {visibleImages.map((gallery, index) => (
                            <MovieGalleryImage
                                key={index}
                                index={index}
                                gallery={gallery}
                                handleImageClick={() => handleImageClick(index)}
                                isSelected={selectedImageIndex === index}
                                styles={styles}
                            />
                        ))}

                        {isModalOpen && createPortal(
                            <ImageModal
                                onClose={handleCloseModal}
                                currentImageIndex={currentImageIndex}
                                handleImageSlider={handleImageSlider}
                                images={galleryImages}
                                styles={styles}
                            />,
                            document.getElementById("modal-root")
                        )}
                    </div>

                    {galleryImages.length > 20 && (
                        <button className={styles["show-more-btn"]} onClick={() => setShowAllImages(prev => !prev)}>
                            {showAllImages ? "Show Less" : "Show More"}
                        </button>
                    )}
                </>
            ) : (
                <FailedToFetchMovies message={"No images available."} />
            )}
        </div>
    );
}

export default MovieGallery