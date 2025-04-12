document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const checkboxes = document.querySelectorAll('input[type="checkbox"][name^="Meals"]');
    const errorContainer = document.createElement("div");
    errorContainer.classList.add("text-danger");
    form.insertBefore(errorContainer, form.querySelector("button[type='submit']"));

    form.addEventListener("submit", function (e) {
        let anySelected = Array.from(checkboxes).some(cb => cb.checked);

        if (!anySelected) {
            e.preventDefault();
            errorContainer.textContent = "Du måste välja minst en måltid.";
        } else {
            errorContainer.textContent = "";
        }
    });
});
