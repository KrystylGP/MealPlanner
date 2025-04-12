document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const ingredientSelect = form.querySelector("select[name='IngredientId'], input[name='IngredientId']");
    const quantityInput = form.querySelector("input[name='Quantity']");
    const errorContainer = document.createElement("div");
    errorContainer.classList.add("text-danger", "mb-2");
    form.insertBefore(errorContainer, form.querySelector("button[type='submit']"));

    form.addEventListener("submit", function (e) {
        let errors = [];

        if (!ingredientSelect.value || parseInt(ingredientSelect.value) <= 0) {
            errors.push("Du måste välja en ingrediens.");
        }

        if (!quantityInput.value || parseInt(quantityInput.value) < 1) {
            errors.push("Mängden måste vara minst 1.");
        }

        if (errors.length > 0) {
            e.preventDefault();
            errorContainer.innerHTML = errors.map(e => `<div>${e}</div>`).join("");
        } else {
            errorContainer.innerHTML = "";
        }
    });
});
