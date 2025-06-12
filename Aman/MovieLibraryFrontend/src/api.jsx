import axios from "axios";
import { config } from "./utils/axiosConfig";
import { envVar } from "./utils/env-var";

// export const fetchMediaList = async (mediaType = "movie", page = 1, genreId = null) => {
//     try {
//         const genreParam = genreId ? `&with_genres=${genreId}` : "";
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/discover/${mediaType}?language=${envVar.tmdbApi.language}&page=${page}${genreParam}`,
//             config);
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const fetchMediaList = async (mediaType = "movie", page = 1, genreId = null) => {
    try {
        let apiRoute = `${envVar.reviewApi.baseUrl}/${mediaType}/list`
        const param = genreId ? `?genreId=${genreId}&pageNumber=${page}` : `?pageNumber=${page}`;

        apiRoute = apiRoute.concat(param);
        const response = await axios.get(apiRoute);
        return response;
    } catch (error) {
        return error;
    }
}

// export const searchMediaList = async (mediaType = "movie", query, page) => {
//     try {
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/search/${mediaType}?query=${query}&language=${envVar.tmdbApi.language}&page=${page}`,
//             config
//         );
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const searchMediaList = async (mediaType = "movie", query, page) => {
    try {
        const response = await axios.get(`${envVar.reviewApi.baseUrl}/${mediaType}/${query}/search?page=${page}`);
        return response;
    } catch (error) {
        return error;
    }
}

// export const fetchMediaDetail = async (mediaType = "movie", movieId) => {
//     try {
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/${mediaType}/${movieId}?language=${envVar.tmdbApi.language}`,
//             config
//         );
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const fetchMediaDetail = async (mediaType = "movie", movieId) => {
    try {
        const response = await axios.get(`${envVar.reviewApi.baseUrl}/${mediaType}/${movieId}/detail`);
        return response;
    } catch (error) {
        return error;
    }
}

// export const fetchMediaTrailer = async (mediaType = "movie", movieId) => {
//     try {
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/${mediaType}/${movieId}/videos?language=${envVar.tmdbApi.language}`,
//             config
//         );
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const fetchMediaTrailer = async (mediaType = "movie", movieId) => {
    try {
        const response = await axios.get(`${envVar.reviewApi.baseUrl}/${mediaType}/${movieId}/trailer`);
        return response;
    } catch (error) {
        return error;
    }
}

export const fetchMediaReviews = async (mediaType = "movie", movieId) => {
    try {
        const response = await axios.get(`${envVar.reviewApi.baseUrl}/${mediaType}/${movieId}/reviews`);
        return response;
    } catch (error) {
        return error;
    }
}

// export const fetchMediaImages = async (mediaType = "movie", movieId) => {
//     try {
//         const response = await axios.get(`${envVar.tmdbApi.baseUrl}/3/${mediaType}/${movieId}/images`, config);
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const fetchMediaImages = async (mediaType = "movie", movieId) => {
    try {
        const response = await axios.get(`${envVar.reviewApi.baseUrl}/${mediaType}/${movieId}/images`);
        return response;
    } catch (error) {
        return error;
    }
}

export const submitMediaReview = async (mediaType = "movie", movieId, data) => {
    try {
        const response = await axios.post(`${envVar.reviewApi.baseUrl}/${mediaType}/${movieId}/reviews`, data);
        return response;
    } catch (error) {
        return error;
    }
}

// export const fetchSimilarMediaList = async (mediaType = "movie", movieId, page) => {
//     try {
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/${mediaType}/${movieId}/similar?language=${envVar.tmdbApi.language}&page=${page}`,
//             config
//         );
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

// export const fetchGenreListOfMedia = async (mediaType = "movie") => {
//     try {
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/genre/${mediaType}/list?language=${envVar.tmdbApi.language}`,
//             config
//         );
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const fetchSimilarMediaList = async (mediaType = "movie", movieId, page) => {
    try {
        const response = await axios.get(
            `${envVar.reviewApi.baseUrl}/${mediaType}/${movieId}/similar?pageNumber=${page}`,
            config
        );
        return response;
    } catch (error) {
        return error;
    }
}

// export const fetchGenreListOfMedia = async (mediaType = "movie") => {
//     try {
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/genre/${mediaType}/list?language=${envVar.tmdbApi.language}`,
//             config
//         );
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const fetchGenreListOfMedia = async (mediaType = "movie") => {
    try {
        const response = await axios.get(`${envVar.reviewApi.baseUrl}/${mediaType}/genre`);
        return response;
    } catch (error) {
        return error;
    }
}

// export const fetchSortByListOfMedia = async (mediaType, sortBy, page) => {
//     try {
//         const response = await axios.get(
//             `${envVar.tmdbApi.baseUrl}/3/${mediaType}/${sortBy}?language=${envVar.tmdbApi.language}&page=${page}`,
//             config
//         );
//         return response;
//     } catch (error) {
//         return error;
//     }
// }

export const fetchSortByListOfMedia = async (mediaType, sortBy, page) => {
    try {
        const response = await axios.get(`${envVar.reviewApi.baseUrl}/${mediaType}/${sortBy}?pageNumber=${page}`);
        return response;
    } catch (error) {
        return error;
    }
}