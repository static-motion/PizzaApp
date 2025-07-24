document.addEventListener('DOMContentLoaded', function () {
    // --- Element References ---
    const minusBtn = document.getElementById('minus-btn');
    const plusBtn = document.getElementById('plus-btn');
    const quantityInput = document.getElementById('quantity-input');
    const totalPriceOutput = document.getElementById('total-price');
    const basePrice = parseFloat(document.getElementById('base-price').value);

    // --- Functions ---

    /**
     * Updates the total price based on the current quantity.
     */
    function updateTotalPrice() {
        const quantity = parseInt(quantityInput.value, 10);
        if (isNaN(quantity) || quantity < 1) {
            return; // Don't update if quantity is invalid
        }
        const total = basePrice * quantity;
        totalPriceOutput.textContent = '$' + total.toFixed(2);
    }

    // --- Event Listeners ---

    // Decrease quantity
    minusBtn.addEventListener('click', function () {
        let currentValue = parseInt(quantityInput.value, 10);
        if (currentValue > 1) {
            quantityInput.value = currentValue - 1;
            updateTotalPrice();
        }
    });

    // Increase quantity
    plusBtn.addEventListener('click', function () {
        let currentValue = parseInt(quantityInput.value, 10);
        quantityInput.value = currentValue + 1;
        updateTotalPrice();
    });

    // Handle manual input changes
    quantityInput.addEventListener('input', function () {
        // Ensure the value is not empty or less than 1
        if (quantityInput.value === '' || parseInt(quantityInput.value, 10) < 1) {
            quantityInput.value = 1;
        }
        updateTotalPrice();
    });

    quantityInput.addEventListener('change', (event) => {
        updateTotalPrice();
    });

    // Ensure the initial total is correct (it's set by the server, but good practice)
    updateTotalPrice();
});