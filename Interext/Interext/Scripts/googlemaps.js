var autocomplete;
var pac_input;
$(function () {

    if (pac_input != null) {
        (function pacSelectFirst(input) {
            // store the original event binding function
            var _addEventListener = (input.addEventListener) ? input.addEventListener : input.attachEvent;

            function addEventListenerWrapper(type, listener) {
                // Simulate a 'down arrow' keypress on hitting 'return' when no pac suggestion is selected,
                // and then trigger the original listener.

                if (type == "keydown") {
                    var orig_listener = listener;
                    listener = function (event) {
                        console.info(pac_input.nodeValue)
                        var suggestion_selected = $(".pac-item-selected").length > 0;
                        if (event.which == 13 && !suggestion_selected) {
                            var simulated_downarrow = $.Event("keydown", { keyCode: 40, which: 40 })
                            orig_listener.apply(input, [simulated_downarrow]);
                        }

                        orig_listener.apply(input, [event]);
                    };
                }

                // add the modified listener
                _addEventListener.apply(input, [type, listener]);
            }

            if (input.addEventListener)
                input.addEventListener = addEventListenerWrapper;
            else if (input.attachEvent)
                input.attachEvent = addEventListenerWrapper;

        })(pac_input);


    }
});


$(document).ready(function () {
    pac_input = document.getElementById("locationSearchTextField");
    autocomplete = new google.maps.places.Autocomplete(pac_input);

});

function initplacechangecevent(functiontorun) {
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        place = autocomplete.getPlace();
        functiontorun(place.formatted_address);
    });
}
