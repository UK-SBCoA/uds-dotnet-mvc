/*
Docuementation

Step 1: Apply "data-assign-parent" to the input element you wish to use
as the group parent (as of now, checkbox type inputs are only supported)

Step 2: Apply "data-assign-child" and "data-assign-value" to the elements you
wish to be modified by the group parent. "data-assign-child" will match the
"data-assign-parent" name of the group parent. Text and number values are
accepted into the "data-assign-value", if you wish to disable the field,
instead of set a value provide the string 'disable'.

- Make sure to have the ManualValueAssign.js loaded last in your <script>
order in the view. Including other client side js before will overwite some
of the disable logic on load

*/



$(() => {
    class ManualValueAssign {
        constructor() {
            this.allParents = $('[data-assign-parent]').toArray();
        }

        /**
         * This function will run through each element with "data-assign-parent"
         * and find their matching "data-assign-child" elements to set their
         * value using "data-assign-value"
         * 
         * @param {string} parent - data-assign-parent value
         * @param {boolean} parentIsChecked - parent checkbox state
         */
        SetManualValues = (parent, parentIsChecked) => {
            let children = $(`[data-assign-child=${parent}]`).toArray();

            children.forEach((child) => {
                let childAssignValue = $(child).data('assign-value');
                let childName = $(child).attr('name');
                let disableChild = false;

                if (parentIsChecked) {
                    if (childAssignValue != "disable") {
                        //set radio buttons groups
                        $(`input[name="${childName}"][type="radio"][value="${childAssignValue}"]`).prop('checked', true);
                        $(`input[name="${childName}"][type="text"]`).val(childAssignValue);
                    } else {
                        disableChild = true;
                    }
                }

                $(`input[name="${childName}"]`).prop('disabled', disableChild);
            })
        }

        /**
        * React to parent value change and set data occoringly
        */
        watchParentValue = (elem) => {
            let parent = $(elem.target).data('assign-parent');
            let parentIsChecked = false;

            if ($(elem.target).is(':checked')) {
                parentIsChecked = true;
            }
            manualValueAssign.SetManualValues(parent, parentIsChecked);
        }

        /**
        * React to child value change and set data occoringly
        */
        watchChildValue = (elem) => {
            let inputGroup = ($(elem.target).data('assign-child'));
            $(`[data-assign-parent="${inputGroup}"]`).prop('checked', false);
        }

        /**
        * Onload function for applying logic with existing data before user
        * interaction
        */
        onLoad = () => {
            manualValueAssign.allParents.forEach((elem) => {
                let parent = $(elem).data('assign-parent');
                manualValueAssign.SetManualValues(parent, $(elem).is(':checked'))
            })
        }
    }

    let manualValueAssign = new ManualValueAssign();

    $('[data-assign-parent]').change((elem) => {
        manualValueAssign.watchParentValue(elem);
    });

    $('[data-assign-child]').change((elem) => {
        manualValueAssign.watchChildValue(elem);
    })

    manualValueAssign.onLoad();
})