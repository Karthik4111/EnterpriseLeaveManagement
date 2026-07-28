import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import VisibilityIcon from "@mui/icons-material/Visibility";

import {
    IconButton,
    Stack,
    Tooltip,
} from "@mui/material";

interface TableActionsProps {
    onView?: () => void;
    onEdit?: () => void;
    onDelete?: () => void;
}

export default function TableActions({
    onView,
    onEdit,
    onDelete,
}: TableActionsProps) {
    return (
        <Stack
            direction="row"
            spacing={1}
        >
            {onView && (
                <Tooltip title="View">
                    <IconButton
                        size="small"
                        color="primary"
                        onClick={onView}
                    >
                        <VisibilityIcon fontSize="small" />
                    </IconButton>
                </Tooltip>
            )}

            {onEdit && (
                <Tooltip title="Edit">
                    <IconButton
                        size="small"
                        color="warning"
                        onClick={onEdit}
                    >
                        <EditIcon fontSize="small" />
                    </IconButton>
                </Tooltip>
            )}

            {onDelete && (
                <Tooltip title="Delete">
                    <IconButton
                        size="small"
                        color="error"
                        onClick={onDelete}
                    >
                        <DeleteIcon fontSize="small" />
                    </IconButton>
                </Tooltip>
            )}
        </Stack>
    );
}