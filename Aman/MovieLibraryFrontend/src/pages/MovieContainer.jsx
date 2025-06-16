import { useCallback, useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";

import "../css/index.css";
import Loader from "../components/Loader.jsx";
import Header from "../components/Header.jsx";
import Pagination from "../components/Pagination.jsx";
import NotFound from "../components/NotFound.jsx";
import MovieListCard from "../components/MovieListCard.jsx";
import FailedToFetchMovies from "../components/FailedToFetchMovies.jsx";

import { fetchMediaList, fetchSortByListOfMedia, searchMediaList } from "../api.jsx";

const MovieContainer = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  const searchTerm = searchParams.get("search") || "";
  const mediaType = searchParams.get("mediaType") || "movie";
  const selectedGenreId = searchParams.get("genreId") ? Number(searchParams.get("genreId")) : null;
  const selectedSortId = searchParams.get("sortId") || null;
  const currentPage = Number(searchParams.get("page")) || 1;

  const [mediaList, setMediaList] = useState([]);
  const [totalPage, setTotalPage] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [noResults, setNoResults] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      setError("");
      let response;

      if (selectedSortId) {
        response = await fetchSortByListOfMedia(mediaType, selectedSortId, currentPage);
      } else if (searchTerm) {
        response = await searchMediaList(mediaType, searchTerm, currentPage);
      } else {
        response = await fetchMediaList(mediaType, currentPage, selectedGenreId);
      }

      // if (response?.status === 200) {
      //   setMediaList(response.data.results || []);
      //   setTotalPage(response.data.total_pages || 0);
      //   setNoResults(!response.data.results.length);
      // } else {
      //   const message = response?.response?.data?.status_message;
      //   setError(message || "Failed to fetch data.")
      //   setMediaList([]);
      //   setNoResults(false);
      // }

      if (response?.status === 200) {
        if (response.data.data.results.length > 0) {
          setMediaList(response.data.data.results || []);
          setTotalPage(response.data.data.total_pages || 0);
          setNoResults(!response.data.data.results.length);
        } else {
          setMediaList([]);
          setNoResults(false);
          setError(response?.data?.data?.status_message) 
        }
      } else {
        const message = response?.response?.data?.errorDetails;
        setError(message || "Failed to fetch data.")
        setMediaList([]);
        setNoResults(false);
      }

      setIsLoading(false);
    };
    const handler = setTimeout(() => {
      fetchData();
    }, 500);
    return () => clearTimeout(handler);
  }, [mediaType, searchTerm, selectedGenreId, selectedSortId, currentPage]);

  const buildQueryString = (params) => {
    const searchParams = new URLSearchParams();
    Object.entries(params).forEach(([key, value]) => {
      if (value !== null && value !== undefined && value !== "") {
        searchParams.set(key, value);
      }
    });
    return `?${searchParams.toString()}`;
  };

  const handleSearch = useCallback((query) => {
    const params = {
      page: 1,
      search: query || undefined,
      mediaType
    };
    navigate(buildQueryString(params));
  }, [mediaType, navigate]);

  const handlePageChange = useCallback((page) => {
    const params = {
      page,
      mediaType,
      search: searchTerm || undefined,
      genreId: selectedSortId ? undefined : selectedGenreId || undefined,
      sortId: selectedSortId || undefined,
    };
    navigate(buildQueryString(params));
  }, [mediaType, searchTerm, selectedGenreId, selectedSortId, navigate]);

  const handleMediaTypeChange = useCallback((type) => {
    const params = {
      mediaType: type,
      page: 1,
      search: searchTerm || undefined,
      sortId: selectedSortId || undefined
    };
    navigate(buildQueryString(params));
  }, [searchTerm, selectedSortId, navigate]);

  return (
    <>
      <Header
        searchTerm={searchTerm}
        onSearch={handleSearch}
        mediaType={mediaType}
        onMediaTypeChange={handleMediaTypeChange}
        selectedGenreId={selectedGenreId}
        selectedSortId={selectedSortId} />
      {isLoading ? (
        <Loader />
      ) : noResults ? (
        <NotFound />
      ) : error ? (
        <>
          <FailedToFetchMovies message={error} />
          <Pagination
            currentPage={currentPage}
            totalPage={totalPage}
            onPageChange={handlePageChange}
          />
        </>
      ) : (
        <>
          <div className="movie-grid">
            {mediaList.map((media) => (
              <MovieListCard
                key={media.id}
                media={media}
                currentPage={currentPage}
                mediaType={mediaType} />
            ))}
          </div>
          <Pagination
            currentPage={currentPage}
            totalPage={totalPage}
            onPageChange={handlePageChange}
          />
        </>
      )}
    </>
  );
};

export default MovieContainer;
