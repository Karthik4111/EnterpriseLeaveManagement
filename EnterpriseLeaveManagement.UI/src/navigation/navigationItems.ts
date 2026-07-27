import DashboardIcon from "@mui/icons-material/Dashboard";
import GroupsIcon from "@mui/icons-material/Groups";
import ApartmentIcon from "@mui/icons-material/Apartment";
import CategoryIcon from "@mui/icons-material/Category";
import EventNoteIcon from "@mui/icons-material/EventNote";
import AssignmentIcon from "@mui/icons-material/Assignment";
import SettingsIcon from "@mui/icons-material/Settings";
import { ROUTES } from "@/constants/routes";
import type { NavigationItem } from "./types";

export const navigationItems: NavigationItem[] = [
    {
        title: "Dashboard",
        path: ROUTES.Dashboard,
        icon: DashboardIcon,
    },
    {
        title: "Employees",
        path: ROUTES.Employees,
        icon: GroupsIcon,
    },
    {
        title: "Departments",
        path: ROUTES.Departments,
        icon: ApartmentIcon,
    },
    {
        title: "Leave Types",
        path: ROUTES.LeaveTypes,
        icon: CategoryIcon,
    },
    {
        title: "Leave Allocations",
        path: ROUTES.LeaveAllocations,
        icon: EventNoteIcon,
    },
    {
        title: "Leave Requests",
        path: ROUTES.LeaveRequests,
        icon: AssignmentIcon,
    },
    {
        title: "Settings",
        path: ROUTES.Settings,
        icon: SettingsIcon,
    },
];