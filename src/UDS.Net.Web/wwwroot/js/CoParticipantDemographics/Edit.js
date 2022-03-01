// With the FVP, if #3 is false, then force disable 4a even if 4 was previously 1
$(function() {
    $('input[name="IsNewCoParticipant"]').on('change', () => {
        $('input[name="EthnicOrigins"]').prop('disabled', true)
        $('input[name="HispanicLatinoEthnicity"]').prop('checked', false)
    })
})
