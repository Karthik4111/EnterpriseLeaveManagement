import { Button, Stack, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

import { ROUTES } from "@/constants/routes";

export default function UnauthorizedPage() {
	const navigate = useNavigate();

	return (
		<Stack
			spacing={2}
			sx={{
				minHeight: "100vh",
				alignItems: "center",
				justifyContent: "center",
				px: 2,
				textAlign: "center",
			}}
		>
			<Typography variant="h4">
				Access Denied
			</Typography>

			<Typography color="text.secondary">
				You do not have permission to open this page.
			</Typography>

			<Stack direction="row" spacing={2}>
				<Button
					variant="outlined"
					onClick={() =>
						navigate(ROUTES.DASHBOARD)
					}
				>
					Go To Dashboard
				</Button>

				<Button
					variant="contained"
					onClick={() =>
						navigate(ROUTES.LOGIN)
					}
				>
					Back To Login
				</Button>
			</Stack>
		</Stack>
	);
}
