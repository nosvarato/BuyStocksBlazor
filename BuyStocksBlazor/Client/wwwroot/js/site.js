window.BlazorStock = {
    addBodyClass: function (Classname) {

        document.body.classList.add(Classname);
        return true;
    },
    removeBodyClass: function (Classname) {

        document.body.classList.remove(Classname);
        return true;
    },
    printDiv: function print(DivPrint) {
        printJS({
            printable: DivPrint,
            type: 'html',
            targetStyles: ['*']
        })
    },
    saveAsFile: function saveAsFile(filename, data) {

        // Create the URL
        const file = new File([data], filename, { type: 'application / octet - stream' });
        const exportUrl = URL.createObjectURL(file);

        // Create the <a> element and click on it
        const a = document.createElement("a");
        document.body.appendChild(a);
        a.href = exportUrl;
        a.download = filename;
        a.target = "_self";
        a.click();

        URL.revokeObjectURL(exportUrl);

        return true;

    },
    confirm: function confirm(message) {
        alert(message);
    }
}