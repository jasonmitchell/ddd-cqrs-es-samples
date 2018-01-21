#!/bin/bash

projection_exists() {
    PROJECTION_NAME=$1

    HTTP_STATUS="$(curl -s -o /dev/null -w "%{http_code}\n" http://127.0.0.1:2113/projection/$PROJECTION_NAME)"
    [ $HTTP_STATUS == "200" ]
}

post_projection() {
    PROJECTION_NAME=$1
    EMIT_ENABLED=$2
    CHECKPOINTS_ENABLED=$3
    TRACK_EMITTED_STREAMS=$4
    ENABLED=$5

    echo POST $PROJECTION_NAME...

    curl -i -X POST \
    "http://127.0.0.1:2113/projections/continuous?name=$PROJECTION_NAME&emit=$EMIT_ENABLED&checkpoints=$CHECKPOINTS_ENABLED&enabled=$ENABLED&trackemittedstreams=$TRACK_EMITTED_STREAMS" \
    -H "authorization: Basic YWRtaW46Y2hhbmdlaXQ=" \
    -H "content-type: application/json" \
    --data-binary @./eventstore/$PROJECTION_NAME.js
}

put_projection() {
    PROJECTION_NAME=$1
    EMIT_ENABLED=$2
    CHECKPOINTS_ENABLED=$3
    TRACK_EMITTED_STREAMS=$4
    ENABLED=$5

    echo PUT $PROJECTION_NAME...

    curl -i -X PUT \
    "http://127.0.0.1:2113/projection/$PROJECTION_NAME/query?emit=$EMIT_ENABLED&checkpoints=$CHECKPOINTS_ENABLED&enabled=$ENABLED&trackemittedstreams=$TRACK_EMITTED_STREAMS" \
    -H "authorization: Basic YWRtaW46Y2hhbmdlaXQ=" \
    -H "content-type: application/json" \
    --data-binary @./eventstore/$PROJECTION_NAME.js
}

deploy_projection() {
    PROJECTION_NAME=$1
    EMIT_ENABLED=${2-no}
    CHECKPOINTS_ENABLED=${3-yes}
    TRACK_EMITTED_STREAMS=${4-no}
    ENABLED=${5-yes}

    echo ""

    if projection_exists $PROJECTION_NAME
    then
        echo Projection $PROJECTION_NAME already exists and will be updated...
        put_projection $PROJECTION_NAME $EMIT_ENABLED $CHECKPOINTS_ENABLED $TRACK_EMITTED_STREAMS $ENABLED
    else
        echo Projection $PROJECTION_NAME does not exist and will be created...
        post_projection $PROJECTION_NAME $EMIT_ENABLED $CHECKPOINTS_ENABLED $TRACK_EMITTED_STREAMS $ENABLED
    fi

    echo ""
}
 
echo Deploying projections...

deploy_projection reservation_summary no yes no yes