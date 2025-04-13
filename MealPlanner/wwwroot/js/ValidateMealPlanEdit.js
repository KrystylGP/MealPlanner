document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const titleInput = form.querySelector('input[name="Title"]');
    const startDateInput = form.querySelector('input[name="StartDate"]');
    const endDateInput = form.querySelector('input[name="EndDate"]');
    const checkboxes = form.querySelectorAll('input[name="SelectedMealIds"]');
    const errorContainer = document.createElement("div");

    errorContainer.classList.add("text-danger", "mb-2");
    form.insertBefore(errorContainer, form.querySelector("button[type='submit']"));

    form.addEventListener("submit", function (e) {
        let errors = [];

        if (!titleInput.value.trim()) {
            errors.push("Titel krävs.");
        }

        if (!startDateInput.value) {
            errors.push("Startdatum krävs.");
        }

        if (!endDateInput.value) {
            errors.push("Slutdatum krävs.");
        }

        if (startDateInput.value && endDateInput.value) {
            const start = new Date(startDateInput.value);
            const end = new Date(endDateInput.value);
            if (start > end) {
                errors.push("Startdatum får inte vara efter slutdatum.");
            }
        }

        const anySelected = Array.from(checkboxes).some(cb => cb.checked);
        if (!anySelected) {
            errors.push("Du måste välja minst en måltid.");
        }

        if (errors.length > 0) {
            e.preventDefault();
            errorContainer.innerHTML = errors.map(e => `<div>${e}</div>`).join("");
        } else {
            errorContainer.innerHTML = "";
        }
    });
});
