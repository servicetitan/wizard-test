#!/bin/sh

dotnet tool restore --tool-manifest './.config/dotnet-tools.json'
if [ $? -ne 0 ]; then
    exit 1
fi

if [[ -n "$TEST_RESULTS_DIRECTORY" ]]; then
    rm -rf ${TEST_RESULTS_DIRECTORY}/*
fi

dotnet test WizardTest.sln \
      --configuration Release \
      --verbosity=normal \
      --logger 'trx' \
      --results-directory ${TEST_RESULTS_DIRECTORY} \
      --collect:"XPlat Code Coverage" \
      -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
if [ $? -ne 0 ]; then
    exit 1
fi

# dotnet stryker --config-file ./.stryker/stryker-config.json --output ${TEST_RESULTS_DIRECTORY}/stryker-output
# if [ $? -ne 0 ]; then
#     exit 1
# fi

dotnet reportgenerator \
    -reports:"${TEST_RESULTS_DIRECTORY}/**/coverage.cobertura.xml" \
    -targetdir:"${COVERAGE_REPORT_DIRECTORY}" \
    "-reporttypes:HtmlSummary;TeamCitySummary;Cobertura"
if [ $? -ne 0 ]; then
    exit 1
fi

# ls ${TEST_RESULTS_DIRECTORY}/stryker-output

echo "Sleeping for a 60 seconds to allow test results to sync ..."
sleep 60
