import { Component, OnInit } from '@angular/core';
import { Validators, FormControl, FormArray, FormBuilder, FormGroup } from '@angular/forms';
@Component({
  selector: 'app-interface-config',
  templateUrl: './interface-config.component.html',
  styleUrls: ['./interface-config.component.scss']
})
export class InterfaceConfigComponent implements OnInit {

  // UMW: Used 'any' instead FormGroup to avoid TypeScript Compilation
  // error in the view page.
  public interfaceConfigForm: any;
  public propertyList: string[] = [];
  public mockData: any = {
    "interfaceName": "OrderProcess",
    "stages": [
      {
        "name": "RcvPortDecode1",
        "artifactType": "Pipeline",
        "searchProperties": [
          {
            "messageId": [
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties"

              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "batchId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "senderId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "receiverId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "correlationId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ]
          }
        ],
        "customTrackData": [
          {
            "propertyList": [
              {
                "isContext": true,
                "displayName": "AskPrice",
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"

              },
              {
                "isContext": true,
                "displayName": "OrderId",
                "propertyName": "OrderId",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='OrderId' and namespace-uri()='']"

              }
            ]
          }
        ],
        "correlationToken": [
          {
            "isContext": true,
            "propertyName": "ReceivePortName",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          },
          {
            "isContext": true,
            "propertyName": "InterchangeID",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          }

        ]

      },
      {
        "name": "RcvPortResolveParty",
        "artifactType": "Pipeline",
        "searchProperties": [
          {
            "messageId": [
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "batchId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "senderId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "receiverId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "correlationId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ]
          }
        ],
        "customTrackData": [
          {
            "propertyList": [
              {
                "isContext": false,
                "displayName": "AskPrice",
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"

              },
              {
                "isContext": false,
                "displayName": "OrderId",
                "propertyName": "OrderId",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='OrderId' and namespace-uri()='']"

              }
            ]
          }
        ],
        "correlationToken": [
          {
            "isContext": true,
            "propertyName": "ReceivePortName",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          },
          {
            "isContext": true,
            "propertyName": "InterchangeID",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          }

        ]

      },
      {
        "name": "Orchestration1",
        "artifactType": "Pipeline",
        "searchProperties": [
          {
            "messageId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties"

              }
            ],
            "batchId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties"

              }
            ],
            "senderId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties"

              }
            ],
            "receiverId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties"

              }
            ],
            "correlationId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "InterchangeID",
                "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties"

              }
            ]
          }
        ],

        "customTrackData": [
          {
            "propertyList": [
              {
                "isContext": false,
                "displayName": "AskPrice",
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"

              },
              {
                "isContext": false,
                "displayName": "OrderId",
                "propertyName": "OrderId",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='OrderId' and namespace-uri()='']"

              }
            ]
          }
        ],
        "correlationToken": [
          {
            "isContext": true,
            "propertyName": "ReceivePortName",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          },
          {
            "isContext": true,
            "propertyName": "InterchangeID",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          }

        ]

      },
      {
        "name": "SendPortEncode1",
        "artifactType": "Pipeline",
        "searchProperties": [
          {
            "messageId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "batchId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "senderId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "receiverId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "correlationId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ]
          }
        ],
        "customTrackData": [
          {
            "propertyList": [
              {
                "isContext": false,
                "displayName": "AskPrice",
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"

              },
              {
                "isContext": false,
                "displayName": "OrderId",
                "propertyName": "OrderId",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='OrderId' and namespace-uri()='']"

              }
            ]
          }
        ],
        "correlationToken": [
          {
            "isContext": true,
            "propertyName": "ReceivePortName",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          },
          {
            "isContext": true,
            "propertyName": "InterchangeID",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          }

        ]

      },
      {
        "name": "SendPortEncode2",
        "artifactType": "Pipeline",
        "searchProperties": [
          {
            "messageId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "batchId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "senderId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "receiverId": [
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": true,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ],
            "correlationId": [
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              },
              {
                "isContext": false,
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"
              }
            ]
          }
        ],
        "customTrackData": [
          {
            "propertyList": [
              {
                "isContext": false,
                "displayName": "AskPrice",
                "propertyName": "AskPrice",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"

              },
              {
                "isContext": false,
                "displayName": "OrderId",
                "propertyName": "OrderId",
                "propertyNamespace": "",
                "xPath": "/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='OrderId' and namespace-uri()='']"

              }
            ]
          }
        ],
        "correlationToken": [
          {
            "isContext": true,
            "propertyName": "ReceivePortName",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          },
          {
            "isContext": true,
            "propertyName": "InterchangeID",
            "propertyNamespace": "http://schemas.microsoft.com/BizTalk/2003/system-properties",
            "xPath": "",
            "interfaceNames": ""

          }

        ]

      }
    ]
  };
  constructor(private _fb: FormBuilder) {

  }
  ngOnInit(): void {

    var data = {
      name: this.mockData.interfaceName,
      stages: []
    };

    data.stages = this.mockData.stages.map((item) => {
      return {
        name: item.name,
        artifactType: item.artifactType,
        properties: item.searchProperties[0]
          .messageId.map(item2 => {
            return {
              isContext: item2.isContext,
              propertyName: item2.propertyName,
              propertyNamespace: item2.propertyNamespace,
              xPath: item2.xPath,
              displayName: ""
            }
          }),
        messageId: [''],
        batchId: [''],
        senderId: [''],
        receiverId: [''],
        correlationId: ['']
      }
    });

    this.interfaceConfigForm = this._fb.group({
      name: ['', [Validators.required]],
      stages: this._fb.array(
        this.initStageConfig(data.stages)
      )
    });

    this.interfaceConfigForm.setValue(data);
  }

  initStageConfig(stages: any[]) {
    var stageConfigs = []
    stages.forEach((item) => {
      stageConfigs.push(this._fb.group({
        name: ['', Validators.required],
        artifactType: ['', Validators.required],
        properties: this._fb.array([
          this.initProperties(item.properties)
        ]),
        messageId: [['']],
        batchId: [['']],
        senderId: [['']],
        receiverId: [['']],
        correlationId: [['']]
      }))
    })

    return stageConfigs;
  }

  initStageConfig2() {

    return this._fb.group({
      name: ['', Validators.required],
      artifactType: ['', Validators.required],
      // properties: this._fb.array([
      //   this.initProperties()
      // ]),
      messageId: [['']],
      batchId: [['']],
      senderId: [['']],
      receiverId: [['']],
      correlationId: [['']]
    });
  }

  initProperties2() {

    var propertyNameControl: FormControl = new FormControl('', Validators.required);
    propertyNameControl.valueChanges.forEach(
      (value: any) => {
        console.log(value);
        this.propertyList.push(value)
      });
    var propertyConfig = this._fb.group({
      isContext: [false],
      displayName: ['', Validators.required],
      propertyName: propertyNameControl,
      propertyNamespace: ['', Validators.required],
      xPath: ['', Validators.required]
    });
    return propertyConfig;
  }

  initProperties(properties: any[]) {
    var propertiesConfig = [];
    properties.forEach(item => {
      var propertyNameControl: FormControl = new FormControl('', Validators.required);
      propertyNameControl.valueChanges.forEach(
        (value: any) => {
          console.log(value);
          this.propertyList.push(value)
        });
      var propertyConfig = this._fb.group({
        isContext: [false],
        displayName: ['', Validators.required],
        propertyName: propertyNameControl,
        propertyNamespace: ['', Validators.required],
        xPath: ['', Validators.required]
      });
      propertiesConfig.push(propertyConfig)
    })

    return propertiesConfig;
  }

  addStageConfig() {
    const control = <FormArray>this.interfaceConfigForm.controls['stages'];
    control.push(this.initStageConfig2());
  }

  removeStageConfig(i: number) {
    const control = <FormArray>this.interfaceConfigForm.controls['stages'];
    control.removeAt(i);
  }

  addProperty(index: number) {
    const propertiesControl = <FormArray>this.interfaceConfigForm.controls['stages']['controls'][index]['controls']['properties'];
    propertiesControl.push(this.initProperties2());
  }

  removeProperty(index: number) {
    const propertiesControl = <FormArray>this.interfaceConfigForm.controls['stages']['controls'][index]['controls']['properties'];
    propertiesControl.removeAt(index);
  }

  save(model: any) {
    console.log(model);
  }
}