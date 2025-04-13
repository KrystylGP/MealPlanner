document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const nameInput = form.querySelector('input[name="Name"]');
    const timeInput = form.querySelector('input[name="CookingTime"]');
    const ingredientBlocks = form.querySelectorAll(".form-check");
    const errorContainer = document.createElement("div");

    errorContainer.classList.add("text-danger", "mb-2");
    form.insertBefore(errorContainer, form.querySelector("button[type='submit']"));

    form.addEventListener("submit", function (e) {
        let errors = [];

        if (!nameInput.value.trim()) {
            errors.push("Namn krävs.");
        }

        const time = parseInt(timeInput.value, 10);
        if (!time || time <= 0) {
            errors.push("Tillagningstid krävs och måste vara större än 0.");
        }

        let anySelected = false;
        ingredientBlocks.forEach(block => {
            const checkbox = block.querySelector('input[type="checkbox"]');
            const quantity = block.querySelector('input[type="number"]');
            if (checkbox && checkbox.checked) {
                anySelected = true;
                const value = parseInt(quantity.value, 10);
                if (!value || value <= 0) {
                    errors.push(`Mängd krävs för ingrediens: ${block.textContent.trim()}`);
                }
            }
        });

        if (!anySelected) {
            errors.push("Du måste välja minst en ingrediens.");
        }

        if (errors.length > 0) {
            e.preventDefault();
            errorContainer.innerHTML = errors.map(e => `<div>${e}</div>`).join("");
        } else {
            errorContainer.innerHTML = "";
        }
    });
});
