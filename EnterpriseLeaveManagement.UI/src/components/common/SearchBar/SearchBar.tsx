import SearchIcon from "@mui/icons-material/Search";
import {
    InputAdornment,
    TextField,
} from "@mui/material";

interface SearchBarProps {
    value: string;
    placeholder?: string;
    onChange: (value: string) => void;
}

export default function SearchBar({
    value,
    placeholder = "Search...",
    onChange,
}: SearchBarProps) {
    return (
        <TextField
            fullWidth
            size="small"
            value={value}
            placeholder={placeholder}
            onChange={(e) => onChange(e.target.value)}
            slotProps={{
                input: {
                    startAdornment: (
                        <InputAdornment position="start">
                            <SearchIcon />
                        </InputAdornment>
                    ),
                },
            }}
            sx={{
                mb: 3,
                maxWidth: 400,
            }}
        />
    );
}