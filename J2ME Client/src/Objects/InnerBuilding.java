/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Objects;

import Serializer.Utils.FieldDeserializer;
import Serializer.Utils.IProtoDeserializable;
import java.io.IOException;

/**
 *
 * @author Admin
 */
public class InnerBuilding implements IProtoDeserializable {

    private int innerBuildingId;                //proto 1
    private int resourceCountToBuild;           //proto 2
    private int resourceCountToProduceUnit;     //proto 3
    private int resourceTypeToBuild;            //proto 4
        //Coal   = 0, // уголь
        //Gold   = 1, // золото
        //Iron   = 2, // железо
        //Stone  = 3, // камень
        //Wood   = 4  // дерево
    private int resourceTypeToProduceUnit;      //proto 5
        //the same enum as previous
    private int innerBuildingType;              //proto 6
        //Voencomat = 0, //для создания пешек
        //Stable = 1, // для создания коней
        //SchoolOfficers = 2, // для создание офицеров
        //VVU = 3, // для создания лодьи
        //GeneralStaff = 4  // для создания королев
    private String name;                        //proto 7

    public InnerBuilding(){}

    public int getInnerBuildingId() {
        return innerBuildingId;
    }
    public void setInnerBuildingId(int innerBuildingId) {
        this.innerBuildingId = innerBuildingId;
    }
    public int getResourceCountToBuild() {
        return resourceCountToBuild;
    }
    public void setResourceCountToBuild(int resourceCountToBuild) {
        this.resourceCountToBuild = resourceCountToBuild;
    }
    public int getResourceCountToProduceUnit() {
        return resourceCountToProduceUnit;
    }
    public void setResourceCountToProduceUnit(int resourceCountToProduceUnit) {
        this.resourceCountToProduceUnit = resourceCountToProduceUnit;
    }
    public int getResourceTypeToBuild() {
        return resourceTypeToBuild;
    }
    public void setResourceTypeToBuild(int resourceTypeToBuild) {
        this.resourceTypeToBuild = resourceTypeToBuild;
    }
    public int getResourceTypeToProduceUnit() {
        return resourceTypeToProduceUnit;
    }
    public void setResourceTypeToProduceUnit(int resourceTypeToProduceUnit) {
        this.resourceTypeToProduceUnit = resourceTypeToProduceUnit;
    }
    public int getInnerBuildingType() {
        return innerBuildingType;
    }
    public void setInnerBuildingType(int innerBuildingType) {
        this.innerBuildingType = innerBuildingType;
    }
    public String getName() {
        return name;
    }
    public void setName(String name) {
        this.name = name;
    }

    public void LoadFrom(byte[] buffer) throws IOException {
        FieldDeserializer dsr = new FieldDeserializer(buffer);
        innerBuildingId = dsr.readInt(1);
        resourceCountToBuild = dsr.readInt(2);
        resourceCountToProduceUnit = dsr.readInt(3);
        resourceTypeToBuild = dsr.readInt(4);
        resourceTypeToProduceUnit = dsr.readInt(5);
        innerBuildingType = dsr.readInt(6);
        name = dsr.readString(7);
    }


}
