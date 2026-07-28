const appConfig = {
    apiBaseUrl:
        import.meta.env.VITE_API_BASE_URL ??
        "https://localhost:7250/api",
};

export default appConfig;