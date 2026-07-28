import axios from "axios";

import appConfig from "@/config/appConfig";

const axiosClient = axios.create({
    baseURL: appConfig.apiBaseUrl,
    headers: {
        "Content-Type": "application/json",
    },
    timeout: 30000,
});

export default axiosClient;