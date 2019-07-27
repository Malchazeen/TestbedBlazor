window.jsInterop = {
    GetFileData: function (e) {
        const reader = new FileReader();
        return new Promise((resolve, reject) => {
            reader.onerror = () => {
                reader.abort();
                reject("Error parsing file");
            };
            reader.onload = () => resolve(reader.result);
            reader.readAsDataURL(e.files[0]);
        });
    }
};