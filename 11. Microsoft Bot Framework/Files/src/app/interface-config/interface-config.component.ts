import { Component, OnInit } from '@angular/core';
import { Validators, FormArray, FormBuilder, FormGroup } from '@angular/forms';
@Component({
  selector: 'app-interface-config',
  templateUrl: './interface-config.component.html',
  styleUrls: ['./interface-config.component.scss']
})
export class InterfaceConfigComponent implements OnInit {

  // UMW: Used 'any' instead FormGroup to avoid TypeScript Compilation
  // error in the view page.
  public interfaceConfigForm: any;
  constructor(private _fb: FormBuilder) { }
  ngOnInit(): void {
    this.interfaceConfigForm = this._fb.group({
      name: ['', [Validators.required]],
      stages: this._fb.array([
        this.initStageConfig()
      ])
    });
  }

  initStageConfig() {
    return this._fb.group({
      name: ['', Validators.required],
      artifactType: ['', Validators.required],
      properties: this._fb.array([
        this.initProperties()
      ]),
      messageId: [['']],
      batchId: [['']],
      senderId: [['']],
      receiverId: [['']],
      correlationId: [['']]
    })
  }

  initProperties() {
    return this._fb.group({
      isContext: [false],
      displayName: ['', Validators.required],
      propertyName: ['', Validators.required],
      propertyNamespace: ['', Validators.required],
      xPath: ['', Validators.required]
    })
  }

  addStageConfig() {
    const control = <FormArray>this.interfaceConfigForm.controls['stages'];
    control.push(this.initStageConfig());
  }

  removeStageConfig(i: number) {
    const control = <FormArray>this.interfaceConfigForm.controls['stages'];
    control.removeAt(i);
  }

  addProperty(index: number) {
    const propertiesControl = <FormArray>this.interfaceConfigForm.controls['stages']['controls'][index]['controls']['properties'];
    propertiesControl.push(this.initProperties());
  }

  removeProperty(index: number) {
    const propertiesControl = <FormArray>this.interfaceConfigForm.controls['stages']['controls'][index]['controls']['properties'];
    propertiesControl.removeAt(index);
  }

  save(model: any) {
    console.log(model);
  }
}