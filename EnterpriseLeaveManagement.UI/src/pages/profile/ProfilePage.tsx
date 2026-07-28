import {
	Box,
	Button,
	Card,
	CardContent,
	Chip,
	Stack,
	Typography,
} from "@mui/material";
import { useNavigate } from "react-router-dom";

import useAuth from "@/hooks/useAuth";
import { ROUTES } from "@/constants/routes";

export default function ProfilePage() {
	const { auth, logout } = useAuth();
	const navigate = useNavigate();

	return (
		<Box sx={{ maxWidth: 700 }}>
			<Typography variant="h4" sx={{ mb: 3 }}>
				My Profile
			</Typography>

			<Card>
				<CardContent>
					<Stack spacing={2}>
						<Typography variant="h6">
							{auth.user?.fullName ?? "Unknown User"}
						</Typography>

						<Typography color="text.secondary">
							{auth.user?.email ?? "N/A"}
						</Typography>

						<Stack direction="row" spacing={1}>
							<Chip
								color="primary"
								label={
									auth.user?.role ?? "N/A"
								}
							/>
						</Stack>

						<Typography variant="body2" color="text.secondary">
							User Id: {auth.user?.id ?? "N/A"}
						</Typography>

						<Stack direction="row" spacing={2} sx={{ pt: 1 }}>
							<Button
								variant="contained"
								onClick={() => navigate(ROUTES.DASHBOARD)}
							>
								Go To Dashboard
							</Button>

							<Button
								variant="outlined"
								color="error"
								onClick={() => {
									logout();
									navigate(ROUTES.LOGIN);
								}}
							>
								Sign Out
							</Button>
						</Stack>
					</Stack>
				</CardContent>
			</Card>
		</Box>
	);
}
