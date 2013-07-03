if (typeof (jQuery) != undefined) {
    $(function () {
        // Watch for problem type checkboxes being turned on or off
        $(".problem-types").change(function () {
            var checkboxList = this;

            var message = $(".message");
            var currentMessage = message.val();
            var currentMessageIsDefault = (currentMessage == "");
            var replacementMessage = "";

            // Check the new status of each of the checkboxes
            $("span", checkboxList).each(function () {

                // Get the default text for this checkbox and compare it to the current message
                var defaultMessage = $(document.createElement('div')).html($(this).data("default-text")).text();
                currentMessageIsDefault = (currentMessageIsDefault || currentMessage === defaultMessage);

                // Use the default text of the first checked box as a replacement message
                if (!replacementMessage && $("input", this).is(":checked")) {
                    replacementMessage = defaultMessage;
                }
            });

            // Only replace message if it hasn't been edited
            if (currentMessageIsDefault) {
                message.val(replacementMessage);
            }
        });
    });
}