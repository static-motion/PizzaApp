document.addEventListener('DOMContentLoaded', () => {
    const pizzaForm = document.forms['pizza-form'];

    pizzaForm.reset();

    pizzaForm.oninput = _ => {
        let sum = 0;

        pizzaForm.querySelectorAll('input').forEach(box => {
            if (box.checked) sum += +box.getAttribute('data-price');
        })

        pizzaForm.querySelectorAll('select').forEach(select => {
            const val = select.value;
            const option = select[val]
            if (option) {
                console.log(option.getAttribute('data-price'));
                sum += +option.getAttribute('data-price');
            }
        })
        pizzaForm.total.textContent = `$${(Math.round(sum * 100) / 100).toFixed(2)}`
    };
    pizzaForm.onsubmit = e => {
        //e.preventDefault()  // disable submit
    }
});