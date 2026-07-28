import { Box, Card, CardContent, Chip, Typography } from "@mui/material";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import CircleIcon from "@mui/icons-material/Circle";

import type { NotificationItem } from "@/types/leave";

interface UpcomingHolidaysProps {
    notifications: NotificationItem[];
}

function timeAgo(dateStr: string): string {
    const diffMs = Date.now() - new Date(dateStr).getTime();
    const mins = Math.floor(diffMs / 60000);
    const hours = Math.floor(mins / 60);
    const days = Math.floor(hours / 24);
    if (days > 0) return `${days}d ago`;
    if (hours > 0) return `${hours}h ago`;
    if (mins > 0) return `${mins}m ago`;
    return "Just now";
}

export default function UpcomingHolidays({
    notifications,
}: UpcomingHolidaysProps) {
    const recent = notifications.slice(0, 6);
    const unreadCount = notifications.filter((n) => !n.isRead).length;

    return (
        <Card elevation={0} sx={{ height: "100%" }}>
            <CardContent sx={{ p: 3, "&:last-child": { pb: 3 } }}>
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                        mb: 2.5,
                    }}
                >
                    <Box
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                        }}
                    >
                        <Typography
                            variant="h6"
                            sx={{ fontWeight: 600, color: "#111827" }}
                        >
                            Notifications
                        </Typography>
                        {unreadCount > 0 && (
                            <Chip
                                label={unreadCount}
                                size="small"
                                color="error"
                                sx={{
                                    height: 20,
                                    fontSize: "0.6875rem",
                                    fontWeight: 700,
                                    "& .MuiChip-label": { px: 0.75 },
                                }}
                            />
                        )}
                    </Box>
                    <NotificationsNoneIcon
                        sx={{ color: "#9CA3AF", fontSize: 20 }}
                    />
                </Box>

                {recent.length === 0 ? (
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "center",
                            justifyContent: "center",
                            py: 5,
                            gap: 1,
                        }}
                    >
                        <NotificationsNoneIcon
                            sx={{ fontSize: 40, color: "#E5E7EB" }}
                        />
                        <Typography
                            sx={{ color: "#9CA3AF", fontSize: "0.875rem" }}
                        >
                            No notifications
                        </Typography>
                    </Box>
                ) : (
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                        }}
                    >
                        {recent.map((n, index) => (
                            <Box
                                key={n.id}
                                sx={{
                                    display: "flex",
                                    gap: 1.5,
                                    p: 1.25,
                                    borderRadius: "10px",
                                    bgcolor: !n.isRead
                                        ? "#EFF6FF"
                                        : "transparent",
                                    mb:
                                        index < recent.length - 1
                                            ? 0.5
                                            : 0,
                                    transition:
                                        "background-color 0.15s ease",
                                    "&:hover": { bgcolor: "#F9FAFB" },
                                    cursor: "default",
                                }}
                            >
                                <Box
                                    sx={{
                                        flexShrink: 0,
                                        display: "flex",
                                        alignItems: "flex-start",
                                        pt: 0.75,
                                    }}
                                >
                                    <CircleIcon
                                        sx={{
                                            fontSize: 7,
                                            color: !n.isRead
                                                ? "#2563EB"
                                                : "#D1D5DB",
                                        }}
                                    />
                                </Box>

                                <Box sx={{ flex: 1, minWidth: 0 }}>
                                    <Typography
                                        sx={{
                                            fontSize: "0.8125rem",
                                            fontWeight: n.isRead ? 400 : 600,
                                            color: "#111827",
                                            lineHeight: 1.4,
                                            overflow: "hidden",
                                            textOverflow: "ellipsis",
                                            whiteSpace: "nowrap",
                                        }}
                                    >
                                        {n.title}
                                    </Typography>
                                    <Typography
                                        sx={{
                                            fontSize: "0.75rem",
                                            color: "#6B7280",
                                            lineHeight: 1.4,
                                            mt: 0.25,
                                            overflow: "hidden",
                                            textOverflow: "ellipsis",
                                            whiteSpace: "nowrap",
                                        }}
                                    >
                                        {n.message}
                                    </Typography>
                                    <Typography
                                        sx={{
                                            fontSize: "0.6875rem",
                                            color: "#9CA3AF",
                                            mt: 0.25,
                                        }}
                                    >
                                        {timeAgo(n.createdOn)}
                                    </Typography>
                                </Box>
                            </Box>
                        ))}
                    </Box>
                )}
            </CardContent>
        </Card>
    );
}
