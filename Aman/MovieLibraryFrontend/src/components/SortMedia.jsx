import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";

const genres = [{
    id: "popular",
    name: "Popular"
},
{
    id: "top_rated",
    name: "Top Rated"
}];

const SortMedia = ({ disabled }) => {
    const [selectedSortType, setSelectedSortType] = useState(null);
    const [searchParams, setSearchParams] = useSearchParams();

    useEffect(() => {
        const initialSortType = searchParams.get("sortId");
        setSelectedSortType(initialSortType ? initialSortType : null);
    }, [searchParams]);

    const handleChange = (e) => {
        const sortId = e.target.value ? e.target.value : null;
        setSelectedSortType(sortId);

        const params = Object.fromEntries([...searchParams]);
        if (sortId === null) {
            delete params.sortId;
        } else {
            params.sortId = sortId;
            delete params.search;
        }
        params.page = 1;
        setSearchParams(params);
    };

    return (
        <div className="sort-select-wrapper">
            <select className="sort-select"
                value={selectedSortType ?? ""}
                onChange={handleChange}
                disabled={disabled}
            >
                <option value="">
                    Select Sort
                </option>
                {genres.map((option) => (
                    <option key={option.id} value={option.id}>
                        {option.name}
                    </option>
                ))}
            </select>
        </div>
    )
}

export default SortMedia