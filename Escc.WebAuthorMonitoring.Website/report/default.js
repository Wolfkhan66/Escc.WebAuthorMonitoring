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

        // Load TinyMCE for the message. Note that the TinyMCE library needs to load from its real URL 
        // rather than a combined script so that it can find its associated resources.
        if (typeof (tinyMCE) != 'undefined') {
            tinyMCE.init({
                mode: "exact",
                elements: "ctl00_content_message",
                theme: "advanced",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                theme_advanced_buttons1: "bold,bullist,,numlist,link, unlink,formatselect",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "",
                theme_advanced_blockformats: "p,h1,h2,h3,h4,h5,h6,blockquote",
                width: "100%"
            });
        }
    });
}