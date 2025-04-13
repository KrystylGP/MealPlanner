document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const nameInput = form.querySelector('input[name="Name"]');
    const typeSelect = form.querySelector('select[name="Type"]');
    const errorContainer = document.createElement("div");
    errorContainer.classList.add("text-danger", "mb-2");

    form.insertBefore(errorContainer, form.querySelector("button[type='submit']"));

    form.addEventListener("submit", function (e) {
        let errors = [];

        if (!nameInput.value.trim()) {
            errors.push("Namn krävs.");
        }

        if (!typeSelect.value) {
            errors.push("Typ krävs.");
        }

        if (errors.length > 0) {
            e.preventDefault();
            errorContainer.innerHTML = errors.map(err => `<div>${err}</div>`).join("");
        } else {
            errorContainer.innerHTML = "";
        }
    });
});
