import { Injectable } from '@angular/core';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class InterfaceConfigService {
    private url = "";

    getInterfaceConfig(): Promise<any> {
        return Promise.resolve({INTERFACECONFIG});
    }
}

const INTERFACECONFIG: any = {
    interfaceName: "OrderProcess",
    stages: [
        {
            name: "RcvPortDecode1",
            artifactType: "Pipeline",
            searchProperties: [
                {
                    messageId: [
                        {
                            isContext: true,
                            propertyName: "InterchangeID",
                            propertyNamespace: "http://schemas.microsoft.com/BizTalk/2003/system-properties"
                        },
                        {
                            isContext: true,
                            propertyName: "AskPrice",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        }
                    ],
                    batchId: [
                        {
                            isContext: true,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        },
                        {
                            isContext: true,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        }
                    ],
                    senderId: [
                        {
                            isContext: true,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        },
                        {
                            isContext: true,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        }
                    ],
                    receiverId: [
                        {
                            isContext: true,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        },
                        {
                            isContext: true,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        }
                    ],
                    correlationId: [
                        {
                            isContext: false,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        },
                        {
                            isContext: false,
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        }
                    ]
                }
            ],
            customTrackData: [
                {
                    propertyList: [
                        {
                            isContext: true,
                            displayName: "AskPrice",
                            propertyName: "AskPrice",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
                        },
                        {
                            isContext: true,
                            displayName: "OrderId",
                            propertyName: "OrderId",
                            propertyNamespace: "",
                            xPath: "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='OrderId' and namespace-uri()='']"
                        }
                    ]
                }
            ],
            correlationToken: [
                {
                    isContext: true,
                    propertyName: "ReceivePortName",
                    propertyNamespace: "http://schemas.microsoft.com/BizTalk/2003/system-properties",
                    xPath: "",
                    interfaceNames: ""
                },
                {
                    isContext: true,
                    propertyName: "InterchangeID",
                    propertyNamespace: "http://schemas.microsoft.com/BizTalk/2003/system-properties",
                    xPath: "",
                    interfaceNames: ""
                }
            ]
        }
    ]
};