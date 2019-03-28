package com.example.joukkoliikenne;

import android.Manifest;
import android.content.pm.PackageManager;
import android.location.Location;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.FragmentActivity;
import android.os.Bundle;
import android.support.v4.content.ContextCompat;
import android.widget.Toast;

import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.tasks.OnSuccessListener;

public class MapsActivity extends FragmentActivity implements OnMapReadyCallback {

    private GoogleMap mMap;
    private FusedLocationProviderClient fusedLocationClient;
    private static final int MY_PERMISSION_ACCESS_FINE_LOCATION = 2;
    private double latitude = 0;
    private double longitude = 0;
    private boolean mLocationPermissionGranted = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_maps);

        fusedLocationClient = LocationServices.getFusedLocationProviderClient(this);

        // Obtain the SupportMapFragment and get notified when the map is ready to be used.
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        if(requestCode == MY_PERMISSION_ACCESS_FINE_LOCATION){
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED){
                Toast.makeText(this,"Permission granted.", Toast.LENGTH_SHORT).show();
                mLocationPermissionGranted = true;
            } else {
                Toast.makeText(this,"Permission denied.", Toast.LENGTH_SHORT).show();
                mLocationPermissionGranted = false;
            }
        }
        updateLocationUI();  // https://developers.google.com/maps/documentation/android-sdk/current-place-tutorial
    }

    private void getLocationPermission(){
        if (ContextCompat.checkSelfPermission(MapsActivity.this, android.Manifest.permission.ACCESS_FINE_LOCATION)// tarkistaa onko oikeuksia löytää koordinaatit
                != PackageManager.PERMISSION_GRANTED) {
            // Permission is not granted
            mLocationPermissionGranted = false;

            ActivityCompat.requestPermissions(this, // kysytään lupaa hakea koordinaatit jos sitä ei vielä ole annettu
                    new String[]{Manifest.permission.ACCESS_FINE_LOCATION},
                    MY_PERMISSION_ACCESS_FINE_LOCATION);

        }
        else{ // suoriutuu jos on oikeudet
            mLocationPermissionGranted = true;
        }
    }

    private void getDeviceLocation(){
        try{
            if(mLocationPermissionGranted){
                fusedLocationClient.getLastLocation() // hakee koordinaatit https://developer.android.com/training/location/retrieve-current
                        .addOnSuccessListener(this, new OnSuccessListener<Location>() {
                            @Override
                            public void onSuccess(Location location) {
                                // Got last known location. In some rare situations this can be null.
                                if (location != null) {
                                    // Logic to handle location object
                                    latitude = location.getLatitude(); // haen koordinaatit omiin arvoihin
                                    longitude = location.getLongitude();
                                    System.out.println("latitude: " + latitude + "\nlongitude " + longitude);

                                    LatLng newLocation = new LatLng(latitude, longitude); // laitoin tähän löydetyt koordinaatit, jos ei löytynyt koordinaatteja niin defaultit molemmissa 0
                                    float zoom = 15.75f;
                                    mMap.addMarker(new MarkerOptions().position(newLocation).title("You are here!"));
                                    mMap.moveCamera(CameraUpdateFactory.newLatLngZoom(newLocation, zoom));
                                }
                            }
                        });
            }
        } catch (SecurityException e){

        }
    }

    private void updateLocationUI() {
        if (mMap == null) {
            return;
        }
        try {
            if (mLocationPermissionGranted) {
                mMap.setMyLocationEnabled(true);
                mMap.getUiSettings().setMyLocationButtonEnabled(true);
            } else {
                mMap.setMyLocationEnabled(false);
                mMap.getUiSettings().setMyLocationButtonEnabled(false);
                getLocationPermission(); // suorittaa funktion joka kysyy oikeuksia
            }
        } catch (SecurityException e)  {
            //Log.e("Exception: %s", e.getMessage());
        }
    }

    /**
     * Manipulates the map once available.
     * This callback is triggered when the map is ready to be used.
     * This is where we can add markers or lines, add listeners or move the camera. In this case,
     * we just add a marker near Sydney, Australia.
     * If Google Play services is not installed on the device, the user will be prompted to install
     * it inside the SupportMapFragment. This method will only be triggered once the user has
     * installed Google Play services and returned to the app.
     */
    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;

        updateLocationUI(); // funktio, joka katsoo oikeuksien tilan

        getDeviceLocation(); // suorittaa funktion, joka hakee koordinaatit jos on oikeudet siihen

    }
}
