import {
    Skeleton,
    Stack,
} from "@mui/material";

interface LoadingStateProps {
    rows?: number;
}

export default function LoadingState({
    rows = 5,
}: LoadingStateProps) {
    return (
        <Stack spacing={2}>
            {Array.from({ length: rows }).map((_, index) => (
                <Skeleton
                    key={index}
                    variant="rounded"
                    height={55}
                />
            ))}
        </Stack>
    );
}