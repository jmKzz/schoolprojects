package com.example.joukkoliikenne;

import android.Manifest;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.Handler;
import android.preference.PreferenceManager;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity implements LocationListener {       // https://developer.android.com/reference/android/location/LocationListener

    static final int REQUEST_REASON_IKKUNA = 2;
    static final int CLEAR_STOP_LOG_LIST = 3;

    Type collectionType;
    ArrayList<String> lista_log;
    String json = "";
    Gson gson;
    SharedPreferences sharedPref;
    Button button2;

    TextView tv;
    TextView tv2;
    FloatingActionButton fab_add;
    FloatingActionButton fab_neg;
    int passengers = 0;

    float speed = 0;        // metriä sekunnissa
    float aika_stop = System.currentTimeMillis();

    private LocationManager locationManager;
    private String provider;
    boolean mLocationPermissionGranted = false;
    LocationManager lm;

    int sendingTime = 1000; // millisekuntia, paikkaupdatejen lähetysten väliaika
    int sendingTravelDistance = 3; // metriä, jotta paikkaupdateje lähetetään
    int stopDelay = 10000;  // 10 sekunnin viive kyselyikkunalle
    double speedLimit = 0.3;  // nopeus täytyy olla alle 0.3m/s, jotta kyselyikkuna aukeaa

    boolean moving = false;
    boolean askingReason = false;
    boolean intentOpen = false;
    private Handler sHandler = new Handler();
    Geocoder geoCoder;

    double latitude = 0;
    double longitude = 0;
    List<Address> addrList = null;
    String addr = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        gson = new Gson();
        sharedPref= PreferenceManager.getDefaultSharedPreferences(getApplicationContext());

        button2 = findViewById(R.id.button2);
        tv = findViewById(R.id.textView);
        tv2 = findViewById(R.id.textView2);
        fab_add = findViewById(R.id.floatingActionButton);
        fab_neg = findViewById(R.id.floatingActionButton2);

        loadValues();

        lm = (LocationManager) this.getSystemService(this.LOCATION_SERVICE);
        geoCoder = new Geocoder(this);

        button2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intentLogs = new Intent(MainActivity.this, ShowStopLogs.class);
                intentLogs.putStringArrayListExtra("logit", lista_log);
                lm.removeUpdates(MainActivity.this);
                intentOpen = true;
                startActivityForResult(intentLogs, CLEAR_STOP_LOG_LIST);    // CLEAR_STOP_LOG_LIST on sitä varten jos käyttäjä haluaa clearata logit niin listan sisältö täytyy lähettää takaisin pääikkunaan
            }
        });

        fab_add.setOnClickListener(new View.OnClickListener(){
            public void onClick(View view) {
                passengers++;
                tv.setText("" + passengers);
            }
        });

        fab_neg.setOnClickListener(new View.OnClickListener(){
            public void onClick(View view) {
                if(passengers > 0){
                    passengers--;
                }
                else {
                    passengers = 0;
                }
                tv.setText("" + passengers);
            }
        });

        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intentMaps = new Intent(MainActivity.this, MapsActivity.class);
                lm.removeUpdates(MainActivity.this);
                intentOpen = true;
                startActivity(intentMaps);
                /*(Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
                        */
            }
        });

        if (ContextCompat.checkSelfPermission(MainActivity.this, android.Manifest.permission.ACCESS_FINE_LOCATION)// tarkistaa onko oikeuksia löytää koordinaatit
                != PackageManager.PERMISSION_GRANTED) {
            // Permission is not granted
            mLocationPermissionGranted = false;

            ActivityCompat.requestPermissions(this, // kysytään lupaa hakea koordinaatit jos sitä ei vielä ole annettu
                    new String[]{Manifest.permission.ACCESS_FINE_LOCATION},
                    1);

        }
        else{ // suoriutuu jos on oikeudet
            mLocationPermissionGranted = true;
        }


    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        if(requestCode == 1){
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED){
                Toast.makeText(this,"Permission granted.", Toast.LENGTH_SHORT).show();
                mLocationPermissionGranted = true;
            } else {
                Toast.makeText(this,"Permission denied.", Toast.LENGTH_SHORT).show();
                tv2.setText("Sovellus tarvitsee luvan toimiakseen!");
                mLocationPermissionGranted = false;
            }
        }

        try {
            if(mLocationPermissionGranted){
                lm.requestLocationUpdates(LocationManager.GPS_PROVIDER, sendingTime, sendingTravelDistance, this); // sendingTime on minimi lähetysaikaväli millisekunneissa ja sendingTravelDistance minimi lähetysten välinen edetty matka
            }
        }
        catch (SecurityException e){

        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        //passengers = sharedPref.getInt("matkustajat", 0);
        //System.out.println("Oikeudet on: " + mLocationPermissionGranted);
        try {
            if(mLocationPermissionGranted){
                lm.requestLocationUpdates(LocationManager.GPS_PROVIDER, sendingTime, sendingTravelDistance, this); // sendingTime on minimi lähetysaikaväli millisekunneissa ja sendingTravelDistance minimi lähetysten välinen edetty matka
            }
        }
        catch (SecurityException e){

        }

        intentOpen = false;
    }

    @Override
    protected void onPause() {
        super.onPause();
        SharedPreferences.Editor editor = sharedPref.edit();
        editor.putInt("matkustajat", passengers);
        editor.commit();
        lm.removeUpdates(this); // pitäisi pysäyttää location päivitykset, jotta käytettävästä laitteesta ei kuluisi liikaa virtaa
    }



    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onLocationChanged(Location location) {      // suoriutuu kun sijainti vaihtuu

        //System.out.println("NOPEUS: " + speed);
        if(location != null && location.hasSpeed()){ // tarkistetaan onko nopeutta
            speed = location.getSpeed(); // metriä sekunnissa

            moving = true;
            //System.out.println("NOPEUS: " + speed);
            //tv.setText("LÄHETTÄÄ!!!!");
        }

        if(speed <= speedLimit && moving){      // jos speed on tarpeeksi pieni niin voidaan mahdollisesti suorittaa kysely tietyllä delaylla
            moving = false;
            //tv.setText("Nopeus on " + speed);

            latitude = location.getLatitude();
            longitude = location.getLongitude();
            try {
               addrList = geoCoder.getFromLocation(latitude,longitude, 1);
               addr = addrList.get(0).getAddressLine(0);    // varsinainen osoite

            } catch (IOException e){

            }

            if(!askingReason){  // tapahtuu jos kyselyä ei olla jo suorittamassa
                //tv.setText("Menossa Chekkiin");
                askingReason = true;
                startMovementRunnable();
            }
        }

        tv2.setText((int)Math.round(speed*3.6)+"km/h"); // muunnetaan kilometriä tunniksi ja pyöristetään samalla
        //tv.setText(speed + "m/s");
    }

    @Override
    public void onStatusChanged(String s, int i, Bundle bundle) {

    }

    @Override
    public void onProviderEnabled(String s) {

    }

    @Override
    public void onProviderDisabled(String s) {

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        //super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == REQUEST_REASON_IKKUNA && resultCode == RESULT_OK) {
            String loggedStop = data.getStringExtra("loggedStop");
            lista_log.add(loggedStop);  // lisätään activityssä luotu logi listaan
            //tv.setText(loggedStop);

            saveLogs(); // tallennetaan logit

            askingReason = false;
        }

        if (requestCode == CLEAR_STOP_LOG_LIST && resultCode == RESULT_OK){
            lista_log = data.getStringArrayListExtra("uudet_logit");
        }

        try {
            if(mLocationPermissionGranted){
                lm.requestLocationUpdates(LocationManager.GPS_PROVIDER, sendingTime, sendingTravelDistance, this); // sendingTime on minimi lähetysaikaväli millisekunneissa ja sendingTravelDistance minimi lähetysten välinen edetty matka
            }
        }
        catch (SecurityException e){

        }

        intentOpen = false;
    }

    public void startMovementRunnable(){
        sHandler.postDelayed(movementStopped, stopDelay); // suoritetaan movementStopped Runnable funktio stopDelay ajan jälkeen
    }

    private Runnable movementStopped = new Runnable() {
        @Override
        public void run() {
            movementStillStopped(addr); // täällä funktiossa katsotaan onko käyttäjä vielä pysähdyksissä
            addrList.clear();
            addr = "";
        }
    };

    public void movementStillStopped(String a){
        if(!moving && askingReason && speed <= speedLimit && !intentOpen){ // jos käyttäjä on pysähtynyt ja muita activityjä ei ole auki niin voidaan kysyä syytä jne.
            //Toast.makeText(this,"PYSÄHDYIT " + stopDelay/1000 + " SEKUNNIN AJAKSI!!!", Toast.LENGTH_SHORT).show();
            Intent intent = new Intent(MainActivity.this, AskReasonActivity.class);
            intent.putExtra("osoite", a);
            //intent.addFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT|Intent.FLAG_ACTIVITY_NEW_TASK);     // estää intentin launchaamisen useaan kertaan, AskReasonActivityn manifestiin lisätään myös singleInstance
            lm.removeUpdates(MainActivity.this);
            startActivityForResult(intent, REQUEST_REASON_IKKUNA);
            //tv.setText("INTENT PITÄISI AUETA!");
        }
        else{
            //tv.setText("Ehdot eivät toteutuneet!");
            askingReason = false;
        }

        //System.out.println("BOOLEANI ASKINGILLE: " + askingReason);
    }

    private void loadValues(){  // lataa sharedpreferencesiin tallennetut arvot applikaation käynnistyksessä
        //SharedPreferences sharedPreferences = getSharedPreferences("sharedPref");
        //json = sharedPref.getString()
        collectionType = new TypeToken<ArrayList<String>>() {}.getType();

        json = sharedPref.getString("logs", null);
        lista_log = gson.fromJson(json, collectionType); // tekee jsonista String listan

        if(lista_log == null){      // jos sharedPreferencestä ei saatu listalle sisältöä, niin tehdään oletuksena tällainen lista ja tallennetaan se
            lista_log = new ArrayList<>();
            saveLogs();
        }

        int p = sharedPref.getInt("matkustajat", 0);
        passengers = p;
        tv.setText(""+passengers);
    }

    private void saveLogs(){
        SharedPreferences.Editor editor = sharedPref.edit();
        json = gson.toJson(lista_log);  // jsoniin tallennetaan gsonin avulla listan sisältö talteen
        editor.putString("logs", json);
        editor.commit();
    }
}
