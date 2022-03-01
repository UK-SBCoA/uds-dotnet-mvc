$(function () {

    //due to the layout of the questions in the Edit.cshtml, the radioFieldToggle is modified here
    //to loop through the checked questions backwards to avoid unsetting any previously set nested
    //quetsion

    //Ex.question 2. Cardiovascular disease, 2a1 responses are renedered BEFORE 2a options. This will
    //cause the response to 2a to undo the disable/enable set by 2a1.


    //get all checked values and disable/enable related fields
    $(`input[type="radio"]:checked`).map(function () {
        SectionToggle($(this))
    })

    //get all unchecked values and pre disable fields that require it. Ex: Other input fields within radio groups


    function SectionToggle(element) {
        var fieldDisabled = false;

        //toggle with integer values
        if (element.data('toggle-values')) {
            var toggleTargetArray = element.data('toggle-targets').split(',');

            if (element.data('toggle-values').includes(parseInt(element.attr('value')))) {
                fieldDisabled = true;
            } else {
                fieldDisabled = false;
            }

            toggleTargetArray.forEach(target => {
                //disable radio inputs
                $(`input[name="${target}"]`).prop('disabled', fieldDisabled);
                //disable/enable text / number inputs if any exist
                $(`input[name="${target}Other"]`).prop('disabled', fieldDisabled);
            });
        }
    }

    $(`input[type="radio"]`).on("change", function () {
        SectionToggle($(this));
        // loop through check fields backwards after sectionToggle for any related fields effected by toggle

        var checkedRadioArray = $(`input[type="radio"]:checked`).toArray()
        checkedRadioArray.slice(0).reverse().map(function () {
            SectionToggle($(this))
        })
        // loop through disabled fields and disable any fields with valid values, but within disabled groups
        $(`input[type="radio"]:disabled`).map(function () {
            $(`input[name="${this.name}Other"]`).prop('disabled', true);
        })
    })

    $('[data-unknown-fill]').on("change", function (elem) {
        let inputTarget = $(elem.target).data('unknown-fill');
        let inputValue = $(elem.target).data('unknown-value');
        
        $(`input[name="${inputTarget}"]`).val(inputValue)
    })
})