function loadStates() {
    var countryId = $("#CountryId").val();

    $.ajax({
        type: "POST",
        url: "/Employee/GetStatesByCountryId",
        data: { countryId: countryId },
        success: function (data) {
            console.log(data);
            $("#StateId").empty();
            $("#StateId").append($('<option>', {
                value: "",
                text: "Select State"
            }));
            $.each(data, function (index, state) {
                $("#StateId").append($('<option>', {
                    value: state.StateId,
                    text: state.StateName
                }));
            });

            $("#CityId").empty();
            $("#CityId").append($('<option>', {
                value: "",
                text: "Select City"
            }));
        },
        error: function () {
            console.log("Error fetching states");
        }
    });
}

function loadCities() {
    var stateId = $("#StateId").val();

    $.ajax({
        type: "POST",
        url: "/Employee/GetCitiesByStateId",
        data: { stateId: stateId },
        success: function (data) {
            $("#CityId").empty();
            $("#CityId").append($('<option>', {
                value: "",
                text: "Select City"
            }));
            $.each(data, function (index, city) {
                $("#CityId").append($('<option>', {
                    value: city.CityId,
                    text: city.CityName
                }));
            });
        },
        error: function () {
            console.log("Error fetching cities");
        }
    });
}